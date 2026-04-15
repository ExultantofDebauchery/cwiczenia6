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
            },
            new Room(),
            new Room(),
        };
        public IActionResult Get()
        {
            return Ok();
            //404 Not Found
            return NotFound();
        }

        public IActionResult Get([FromRoute] int id)
        {
            var room = rooms.FirstOrDefault(x => x.id == id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateRoomDTO createRoomDTO)
        {
            var room=new Room()
            {
                id=rooms.Count+1,
                Name=createRoomDTO.name,
            };
        }
    }
}
