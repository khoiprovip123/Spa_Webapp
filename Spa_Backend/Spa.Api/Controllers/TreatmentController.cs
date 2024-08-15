using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Commands;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using System.Text.Json.Serialization;
using System.Text.Json;
using DocumentFormat.OpenXml.Presentation;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreatmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITreatmentService _treatmentService;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public TreatmentController(IMediator mediator, ITreatmentService treatmentService)
        {
            _mediator = mediator;
            _treatmentService = treatmentService;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        [HttpGet]
        public async Task<ActionResult> GetTreatmentByCustomer(long customerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var listTreatment = await _treatmentService.GetTreatmentCardAsyncByCustomer(customerId);
                return new JsonResult(listTreatment, _jsonSerializerOptions);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{treatmendID}")]
        public async Task<ActionResult> GetTreatmentDetailByID(long treatmendID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var listTreatment = await _treatmentService.GetTreatmentCardDetailAsyncByID(treatmendID);
                return new JsonResult(listTreatment, _jsonSerializerOptions);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public async Task<ActionResult> CreateTreatmentCard(TreatmentCardDTO treatmentCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var command = new CreateTreatmentCardCommand
                {                 
                    CustomerID = treatmentCard.CustomerID,
                    StartDate = treatmentCard.StartDate,                
                    CreateBy = treatmentCard.CreateBy,
                    Notes = treatmentCard.Notes,
                    TreatmentDetailDTO = treatmentCard.TreatmentDetailDTOs,
                    status = treatmentCard.status                 
                };
                var id = await _mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("{treatmentID}")]
        public async Task<ActionResult> UpdateTreatment(long treatmentID, TreatmentCardDTO treatmentCardDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                TreatmentCard treatmentCard = new TreatmentCard
                {
                    CreateBy = treatmentCardDTO.CreateBy,
                    CustomerID = treatmentCardDTO.CustomerID,
                    StartDate = treatmentCardDTO.StartDate,
                    Notes = treatmentCardDTO.Notes,        
                    Status = treatmentCardDTO.status,
                    TreatmentDetails = treatmentCardDTO.TreatmentDetailDTOs.Select(a => new TreatmentDetail
                    {
                       ServiceID = a.ServiceID,
                       Price = a.Price,
                       Quantity = a.Quantity,
                       IsDone = a.IsDone,
                       QuantityDone = a.QuantityDone,
                       AmountDiscount = a.AmountDiscount,
                       KindofDiscount = a.KindofDiscount,
                       Note =a.Note,
                       TotalAmount = a.TotalAmount,
                    }).ToList(),
                };
                var updateTreatment = await _treatmentService.UpdateTreatment(treatmentID, treatmentCard);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("UpdateStatuSsession")]
        public async Task<ActionResult> UpdateStatusSession(long sessionID, bool status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var update = _treatmentService.UpdateStatusSession(sessionID, status);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("UpdateStatusTreatmentCard")]
        public async Task<ActionResult> UpdateStatusTreatmentCard(long treatmentCardId, string status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var update = await _treatmentService.UpdateStatusTreatmentCard(treatmentCardId, status);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteTreatmentDetail(long treatmentDetailID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                 await _treatmentService.DeleteTreatmentDetail(treatmentDetailID);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
