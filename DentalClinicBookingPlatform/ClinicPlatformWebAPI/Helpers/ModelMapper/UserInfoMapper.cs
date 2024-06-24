﻿using ClinicPlatformDTOs.UserModels;

namespace ClinicPlatformWebAPI.Helpers.ModelMapper
{
    public class UserInfoMapper
    {
        public static UserInfoModel FromCustomerView(CustomerInfoViewModel customerInfo)
        {
            return new UserInfoModel
            {
                CustomerId = customerInfo.CustomerId,
                Email = customerInfo.Email,
                Fullname = customerInfo.Fullname,
                Phone = customerInfo.Phone,
                Sex = customerInfo.Sex,
                Insurance = customerInfo.Insurance,
                Birthdate = customerInfo.Birthdate,
                IsActive = customerInfo.IsActive,
            };
        }

        public static UserInfoModel FromDentistView(DentistInfoViewModel dentistInfo)
        {
            return new UserInfoModel
            {
                DentistId = dentistInfo.DentistId,
                Email = dentistInfo.Email,
                Fullname = dentistInfo.Fullname,
                Phone = dentistInfo.Phone,
                Role = "Dentist",
                ClinicId = dentistInfo.ClinicId,
                IsOwner = dentistInfo.IsOwner,
                IsActive = dentistInfo.IsActive,
            };
        }

        public static CustomerInfoViewModel ToCustomerView(UserInfoModel userInfo)
        {
            return new CustomerInfoViewModel
            {
                CustomerId = userInfo.CustomerId ?? 0,
                Email = userInfo.Email,
                Username = userInfo.Username,
                Phone = userInfo.Phone,
                Sex = userInfo.Sex,
                Fullname = userInfo.Fullname,
                Birthdate = userInfo.Birthdate,
                Insurance = userInfo.Insurance,
                JoinedDate = userInfo.JoinedDate,
                IsActive = userInfo.IsActive,
            };
        }

        public static DentistInfoViewModel ToDentistView(UserInfoModel userInfo)
        {
            return new DentistInfoViewModel
            {
                DentistId = userInfo.DentistId ?? 0,
                Email = userInfo.Email,
                Username = userInfo.Username,
                Phone = userInfo.Phone,
                Fullname = userInfo.Fullname,
                ClinicId = userInfo.ClinicId,
                IsOwner = userInfo.IsOwner ?? false,
                JoinedDate = userInfo.JoinedDate,
                IsActive = userInfo.IsActive,
            };
        }

        public static UserInfoModel FromRegistration(UserRegistrationModel userInfo)
        {
            return new UserInfoModel
            {
                Email = userInfo.Email,
                Username = userInfo.Username,
                PasswordHash = userInfo.Password,
                IsOwner = userInfo.ClinicOwner,
                ClinicId = userInfo.Clinic,
            };
        }
    }
}
