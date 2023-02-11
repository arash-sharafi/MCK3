using System;
using System.ComponentModel.DataAnnotations;

namespace Mock3.Core.Models
{
    public class Exam
    {
        private Exam(string name, string startDate, string description, int capacity, bool isOpen)
        {
            Name = name;
            StartDate = startDate;
            Description = description;
            Capacity = capacity;
            RemainingCapacity = capacity;
            IsOpen = isOpen;
        }

        public int Id { get; private set; }

        [Required]
        [StringLength(100)]
        public string Name { get; private set; }

        [Required]
        [StringLength(10)]
        public string StartDate { get; private set; }

        [StringLength(300)]
        public string Description { get; private set; }

        public int Capacity { get; private set; }

        public int RemainingCapacity { get; private set; }

        public bool IsOpen { get; private set; }

        public void Update(string name, string startDate, string description, int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentNullException(nameof(capacity));

            Name = name ?? throw new ArgumentNullException(nameof(name));
            StartDate = startDate ?? throw new ArgumentNullException(nameof(startDate));
            Capacity = capacity;
            Description = description;
        }

        public void OpenRegistration()
        {
            IsOpen = true;
        }

        public void CloseRegistration()
        {
            IsOpen = false;
        }

        public void FreeSeat()
        {
            if (RemainingCapacity < Capacity)
                ++RemainingCapacity;
        }

        public void ReserveSeat()
        {
            if (RemainingCapacity > 0)
                --RemainingCapacity;
        }


        public static Exam Create(string name, string startDate, string description, int capacity, bool isOpen)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (startDate == null)
                throw new ArgumentNullException(nameof(startDate));

            if (capacity <= 0)
                throw new ArgumentNullException(nameof(capacity));


            return new Exam(name, startDate, description, capacity, isOpen);
        }

        protected Exam()
        {

        }

    }
}