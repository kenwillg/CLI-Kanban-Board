using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tugas2___OOP
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public string AppUsage { get; set; }

        public virtual ICollection<Board> Boards { get; set; }

        public User()
        {
            Boards = new List<Board>();
        }
    }

    public class Board
    {
        [Key]
        public int Id { get; set; }

        public string BoardName { get; set; }
        public string BoardDesc { get; set; }
        public int UserId { get; set; } // Foreign key to User

        public virtual ICollection<Task>? ToDoTasks { get; set; }
        public virtual ICollection<Task>? DoingTasks { get; set; }
        public virtual ICollection<Task>? DoneTasks { get; set; }

        public Board()
        {
            ToDoTasks = new List<Task>();
            DoingTasks = new List<Task>();
            DoneTasks = new List<Task>();
        }

        public Board BoardClone()
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            };

            string json = System.Text.Json.JsonSerializer.Serialize(this, options);
            Board board = System.Text.Json.JsonSerializer.Deserialize<Board>(json, options);

            // Set the Id of the new Board to 0
            board.Id = 0;

            return board;
        }
    }

    public class Task
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        // Foreign Key nya
        public int? ToDoBoardId { get; set; }
        public int? DoingBoardId { get; set; }
        public int? DoneBoardId { get; set; }

        // Navigasi koleksi
        [ForeignKey("ToDoBoardId")]
        public virtual Board ToDoBoard { get; set; }

        [ForeignKey("DoingBoardId")]
        public virtual Board DoingBoard { get; set; }

        [ForeignKey("DoneBoardId")]
        public virtual Board DoneBoard { get; set; }

    }

}
