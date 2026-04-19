namespace cwiczenia6.DTOS;
using System.ComponentModel.DataAnnotations;
public class CreateReservationDTO
{
    public int RoomId { get; set; }
    [Required]
    public string OrganizerName { get; set; }
    [Required]
    public string Topic {get; set;}
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Status { get; set; }
}