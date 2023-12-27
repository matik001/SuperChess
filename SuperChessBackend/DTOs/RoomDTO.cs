using SuperChessBackend.DB.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SuperChessBackend.DTOs {
    public class RoomCreateDTO
    {
        public string RoomName { get; set; }
    }
    public class RoomDTO : RoomCreateDTO
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
    }
    public class RoomExtendedDTO : RoomDTO
    {
        public int AmountOfUsers { get; set; }
    }
}
