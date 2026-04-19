using cwiczenia6.DTOS;
using cwiczenia6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cwiczenia6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        public  static List<Reservation> reservations = new List<Reservation>()
        {
            new Reservation()
            {
                Id=1,
                RoomId = 1,
                OrganizerName = "Adam Smyk",
                Topic = "Panie i panowie",
                Date = new DateOnly(2026,6,1),
                StartTime = new TimeOnly(8,30),
                EndTime = new TimeOnly(21,37),
                Status = "confirmed"
            },
            new Reservation()
            {
                Id=2,
                RoomId = 1,
                OrganizerName = "Michał Tomaszewski",
                Topic = "Robienie gier w Javie i dlaczego nie warto tego robić",
                Date = new DateOnly(2026,6,2),
                StartTime = new TimeOnly(8,30),
                EndTime = new TimeOnly(14,30),
                Status = "confirmed"
            }
            
        };
        [HttpGet]
        public IActionResult Get([FromQuery]DateOnly? date,[FromQuery]string? status,[FromQuery]int? roomId)
        {
            var query=reservations.AsQueryable();
            if (date.HasValue)
            {
                query=query.Where(r=>r.Date==date.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query=query.Where(r=>r.Status==status);
            }

            if (roomId.HasValue)
            {
                query=query.Where(r=>r.RoomId==roomId.Value);
            }
            return Ok(query.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var reser=reservations.FirstOrDefault(r => r.Id == id);
            if (reser == null)
            {
                return NotFound();
            }
            return Ok(reser);
        }
        [HttpPost]
        public IActionResult Post(CreateReservationDTO dto)
        {
            var room=RoomsController.rooms.FirstOrDefault(r=>r.Id==dto.RoomId);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            if (!room.IsActive)
            {
                return Conflict("Room is not active");
            }
            bool overlap=reservations.Any(r=>r.RoomId==dto.RoomId&&r.Date==dto.Date&&
                                             dto.StartTime<r.EndTime&&r.StartTime<dto.EndTime);
            if (overlap)
            {
                return Conflict("Time Overlap");
            }

            var reservation = new Reservation()
            {
                Id = reservations.Max(r => r.Id) + 1,
                RoomId = dto.RoomId,
                OrganizerName = dto.OrganizerName,
                Topic = dto.Topic,
                Date = dto.Date,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = dto.Status,
            };
            if (dto.EndTime <= reservation.StartTime)
            {
                return BadRequest("EndTime must be greater than StartTime");
            }
            reservations.Add(reservation);
            return CreatedAtAction(nameof(Get), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CreateReservationDTO dto)
        {
            var reservation=reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto.EndTime <= reservation.StartTime)
            {
                return BadRequest("EndTime must be greater than StartTime");
            }
            reservation.RoomId = dto.RoomId;
            reservation.OrganizerName=dto.OrganizerName;
            reservation.Topic=dto.Topic;
            reservation.Status=dto.Status;
            reservation.Date=dto.Date;
            reservation.StartTime=dto.StartTime;
            reservation.EndTime=dto.EndTime;
            return Ok(reservation);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var reservation=reservations.FirstOrDefault(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            reservations.Remove(reservation);
            return NoContent();
        }
    }
}
