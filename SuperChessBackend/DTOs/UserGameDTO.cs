using SuperChessBackend.DB.Repositories;
using SuperChessBackend.DB;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SuperChessBackend.DTOs
{
    public class UserGameDTO
    {
        public int Id { get; set; }
        public PlayerColors Color { get; set; }
        public string Nick { get; set; }
        public string? GuestGuid { get; set; } 


        public UserDTO? User { get; set; }
        public int GameId { get; set; }
    }
}
