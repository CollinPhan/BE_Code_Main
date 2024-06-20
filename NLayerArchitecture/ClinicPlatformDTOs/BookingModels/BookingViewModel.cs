﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicPlatformDTOs.BookingModels
{
    public class BookingViewModel
    {
        public Guid BookId { get; set; }
        public string appointmentType { get; set; } = null!;
        public string CustomerFullName { get; set; } = null!;
        public string DentistFullname { get; set; } = null!;
        public DateOnly AppointmentDate { get; set; }
        public DateTime CreationTime { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        public TimeOnly? ExpectedEndTime { get; set; }
        public string ClinicName { get; set; } = null!;
        public string ClinicAddress { get; set; } = null!;
        public string ClinicPhone { get; set; } = null!;
        public string SelectedServiceName { get; set; } = null!;
        public bool IsRecurring {  get; set; }
    }
}
