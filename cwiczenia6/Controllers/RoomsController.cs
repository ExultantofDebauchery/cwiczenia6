using cwiczenia6.DTOS;
using cwiczenia6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace cwiczenia6.Controllers
{
    //api/rooms
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        public static List<Room> rooms = new List<Room>()
        {
            new Room()
            {
                Id = 1,
                Name="Wykladowa1",
                BuildingCode="A",
                Capacity=250,
                HasProjector=true,
                IsActive=true,
                Floor = 0
            },
            new Room()
            {
                Id = 201,
                Name="laboratorium",
                BuildingCode="B",
                Capacity=35,
                HasProjector=true,
                IsActive=true,
                Floor = 2
            },
            new Room()
            {
                Id = 15,
                Name="Malarnia",
                BuildingCode="C",
                Capacity=15,
                HasProjector=false,
                IsActive=false,
                Floor = 1
            },
        };
        [HttpGet]
        public IActionResult Get([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
        {
            var query=rooms.AsQueryable();
            if (minCapacity.HasValue)
            {
                query = query.Where(x => x.Capacity >= minCapacity.Value);
            }

            if (hasProjector.HasValue)
            {
                query = query.Where(x => x.HasProjector == hasProjector.Value);
            }

            if (activeOnly.HasValue)
            {
                query = query.Where(x => x.IsActive == activeOnly.Value);
            }
            return Ok(query.ToList());
        }
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var room = rooms.FirstOrDefault(x => x.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }
        [HttpGet("building/{buildingCode}")]
        public IActionResult Get([FromRoute] string buildingCode)
        {
            var building = rooms.Where(x => x.BuildingCode == buildingCode).ToList();
            return Ok(building);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateRoomDTO createRoomDTO)
        {
            var room=new Room()
            {
                Id=rooms.Max(x=>x.Id)+1,
                Name=createRoomDTO.Name,
                BuildingCode=createRoomDTO.BuildingCode,
                Capacity=createRoomDTO.Capacity,
                HasProjector=createRoomDTO.HasProjector,
                IsActive=createRoomDTO.IsActive,
                Floor=createRoomDTO.Floor,
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            rooms.Add(room);
            return CreatedAtAction(nameof(Get), new { id = room.Id }, room);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CreateRoomDTO createRoomDTO)
        {
            var room=rooms.FirstOrDefault(x => x.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            room.Name = createRoomDTO.Name;
            room.Capacity = createRoomDTO.Capacity;
            room.HasProjector = createRoomDTO.HasProjector;
            room.IsActive = createRoomDTO.IsActive;
            room.BuildingCode = createRoomDTO.BuildingCode;
            room.Floor = createRoomDTO.Floor;
            return Ok(room);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var room=rooms.FirstOrDefault(x => x.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            if (ReservationsController.reservations.Any(x => x.RoomId == id))
            {
                return Conflict("There is already a reservation");
            }
            rooms.Remove(room);
            return NoContent();
        }
    }
}
