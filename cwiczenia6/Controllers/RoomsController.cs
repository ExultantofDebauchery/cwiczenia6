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
            },
            new Room()
            {
                Id = 201,
                Name="laboratorium",
                BuildingCode="B",
                Capacity=35,
                HasProjector=true,
                IsActive=true,
            },
            new Room()
            {
                Id = 15,
                Name="Malarnia",
                BuildingCode="C",
                Capacity=15,
                HasProjector=false,
                IsActive=false,
            },
        };
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(rooms);
            //404 Not Found
            return NotFound();
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

        [HttpGet("filter")]
        public IActionResult Filter([FromQuery] int? roomCapacity, [FromQuery] bool? hasProjector,
            [FromQuery] bool? isActive)
        {
            var query=rooms.AsQueryable();
            if (roomCapacity.HasValue)
            {
                query = query.Where(x => x.Capacity >= roomCapacity.Value);
            }

            if (hasProjector.HasValue)
            {
                query = query.Where(x => x.HasProjector == hasProjector.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(x => x.IsActive == isActive.Value);
            }
            return Ok(query.ToList());
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
                Id=rooms.Count+1,
                Name=createRoomDTO.Name,
                BuildingCode=createRoomDTO.BuildingCode,
                Capacity=createRoomDTO.Capacity,
                HasProjector=createRoomDTO.HasProjector,
                IsActive=createRoomDTO.IsActive
            };
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
            room.Capacity = createRoomDTO.Capacity;
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
            rooms.Remove(room);
            return NoContent();
        }
    }
}
