﻿using ClinicPlatformDTOs.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicPlatformDTOs.ClinicModels
{
    public class ClinicInfoModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public TimeOnly? OpenHour { get; set; }
        public TimeOnly? CloseHour { get; set; }
        public IEnumerable<ClinicServiceInfoModel> ClinicServices { get; set; } = [];
        public IEnumerable<ClinicSlotInfoModel> ClinicSlots { get; set; } = [];
        public IEnumerable<ClinicStaffInfoModel> ClinicStaff { get; set; } = [];
        public int? OwnerId;
        public bool? Status { get; set; }
    }
}
