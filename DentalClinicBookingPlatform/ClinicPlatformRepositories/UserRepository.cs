﻿using ClinicPlatformDatabaseObject;
using ClinicPlatformDTOs.UserModels;
using ClinicPlatformRepositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Numerics;

namespace ClinicPlatformRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DentalClinicPlatformContext context;
        private bool disposedValue;

        public UserRepository(DentalClinicPlatformContext context)
        {
            this.context = context; 
        }

        public UserInfoModel? AddUser(UserInfoModel userInfo)
        {
            User user = MapToUser(userInfo);

            user.CreationTime = DateTime.Now;

            context.Users.Add(user);
            context.SaveChanges();
            return MapToUserInfo(user);
        }

        public IEnumerable<UserInfoModel> GetAllUser(bool includeRemoved = true, bool includeInactive = true)
        {
            IEnumerable<User> userList = context.Users.Include(x => x.Customer).Include(x => x.Dentist).ToList();

            if (!includeRemoved)
            {
                userList = userList.Where(x => !x.Removed);
            }

            if (!includeInactive)
            {
                userList = userList.Where(x => x.Active);
            }

            return userList.Select(x => MapToUserInfo(x));
        } 

        public IEnumerable<UserInfoModel> GetUserWithRole(string role) 
        {
            return context.Users
                .Include(x=> x.Customer)
                .Include(x => x.Dentist)
                .Where(x => x.Role == role)
                .Select(x => MapToUserInfo(x)).ToList();
        }

        public IEnumerable<UserInfoModel> GetUserWithCreationDate(DateOnly date)
        {
            return context.Users
                .Include(x => x.Customer)
                .Include(x => x.Dentist)
                .Where(x => DateOnly.FromDateTime(x.CreationTime) == date)
                .Select(x => MapToUserInfo(x));
        }

        public IEnumerable<UserInfoModel> GetRemovedUsers()
        {
            return context.Users
                .Include(x => x.Customer)
                .Include(x => x.Dentist)
                .Where(x => x.Removed)
                .Select(x => MapToUserInfo(x));
        }

        public IEnumerable<UserInfoModel> GetUnactivatedUser() 
        {
            return context.Users
                .Include(x => x.Customer)
                .Include(x => x.Dentist)
                .Where(x => !x.Active)
                .Select(x => MapToUserInfo(x));
        }

        public IEnumerable<UserInfoModel> GetActivatedUser() 
        {
            return context.Users
                .Include(x => x.Customer)
                .Include(x => x.Dentist)
                .Where(x => x.Active)
                .Select(x => MapToUserInfo(x));
        }

        public UserInfoModel? GetUser(int userId)
        {
            User? user = context.Users.Include(x => x.Dentist).Include(x => x.Customer).Where(x => x.Id == userId).FirstOrDefault();

            if (user != null)
            {
                return MapToUserInfo(user);
            }

            return null;
        }

        public UserInfoModel? GetUserWithCustomerID(int customerId)
        {
            return context.Users.Include(x => x.Customer)
                .Where(x => x.Customer != null && x.Customer.Id == customerId)
                .Select(x => MapToUserInfo(x))
                .FirstOrDefault();
        }

        public UserInfoModel? GetUserWithDentistID(int dentistId)
        {
            return context.Users.Include(x => x.Dentist)
                .Where(x => x.Dentist != null && x.Dentist.Id == dentistId)
                .Select(x => MapToUserInfo(x))
                .FirstOrDefault();
        }

        public UserInfoModel? GetUserWithEmail(string email)
        {
            return context.Users.Include(x => x.Customer)
                .Include(x => x.Dentist)
                .Where(x => x.Email == email && !x.Removed)
                .Select(x => MapToUserInfo(x))
                .FirstOrDefault();
        }

        public UserInfoModel? GetUserWithUsername(string username)
        {
            return context.Users.Include(x => x.Customer)
                .Include(x => x.Dentist)
                .Where(x => x.Username == username)
                .Select(x => MapToUserInfo(x))
                .FirstOrDefault();
        }

        public void DeleteUser(int userId)
        {
            User? user = context.Users.Find(userId);

            if (user == null)
            {
                throw new InvalidOperationException($"The user with provided Id {userId} does not exist!");
            }

            user.Removed = true;

            context.Entry(user).CurrentValues.SetValues(user);
            context.SaveChanges();
        }

        public UserInfoModel? UpdateUser(UserInfoModel userInfo)
        {
            User? target = context.Users.Include(x => x.Customer).Include(x => x.Dentist).Where(x => x.Id == userInfo.Id).FirstOrDefault(); ;

            if (target != null)
            {

                target.Username = userInfo.Username ?? target.Username;
                target.PasswordHash = userInfo.PasswordHash ?? target.PasswordHash;
                target.Fullname = userInfo.Fullname ?? target.Fullname;
                target.Email = userInfo.Email ?? target.Email;
                target.Phone = userInfo.Phone ?? target.Phone;
                target.Active = userInfo.IsActive;
                target.Removed = userInfo.IsRemoved;

                if (target.Role == "Dentist")
                {
                    target.Dentist!.ClinicId = userInfo.ClinicId;
                    target.Dentist!.IsOwner = userInfo.IsOwner;
                }

                if (target.Role == "Customer")
                {
                    target.Customer!.Birthdate = userInfo.Birthdate ?? target.Customer.Birthdate;
                    target.Customer!.Insurance = userInfo.Insurance ?? target.Customer.Insurance;
                    target.Customer!.Sex = userInfo.Sex ?? target.Customer.Sex;
                }

                context.Users.Update(target);
                context.SaveChanges();

                return userInfo;
            }

            return null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        static User MapToUser(UserInfoModel userInfo)
        {
            User user = new User()
            {
                Id = userInfo.Id,
                Username = userInfo.Username,
                PasswordHash = userInfo.PasswordHash,
                Salt = userInfo.Salt,
                Email = userInfo.Email,
                Phone = userInfo.Phone,
                Fullname = userInfo.Fullname,
                Active = userInfo.IsActive,
                Removed = userInfo.IsRemoved,
                CreationTime = DateTime.Now,
                Role = userInfo.Role,
            };

            if (userInfo.Role == "Admin")
            {
                user.Active = true;
            }
            else if (userInfo.Role == "Dentist")
            {
                user.Dentist = new Dentist()
                {
                    Id = userInfo.DentistId ?? 0,
                    UserId = userInfo.Id,
                    ClinicId = userInfo.ClinicId,
                    IsOwner = userInfo.IsOwner,
                };
            }
            else if (userInfo.Role == "Customer")
            {
                user.Customer = new Customer()
                {
                    Birthdate = userInfo.Birthdate,
                    Sex = userInfo.Sex,
                    Insurance = userInfo.Insurance,
                };
            }

            return user;
        }

        static UserInfoModel MapToUserInfo(User user)
        {
            UserInfoModel userInfo = new UserInfoModel()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Fullname = user.Fullname!,
                Phone = user.Phone!,
                IsActive = user.Active,
                IsRemoved = user.Removed,
                JoinedDate = user.CreationTime,
                PasswordHash = user.PasswordHash,
                Salt = user.Salt,
                Role = user.Role,
            };

            if (user.Role == "Dentist")
            {
                userInfo.DentistId = user.Dentist?.Id;
                userInfo.ClinicId = user.Dentist?.ClinicId!;
                userInfo.IsOwner = user.Dentist?.IsOwner ?? false;
            }
            else if (user.Role == "Customer")
            {
                userInfo.CustomerId = user.Customer?.Id;
                userInfo.Insurance = user.Customer?.Insurance!;
                userInfo.Sex = user.Customer?.Sex!;
                userInfo.Birthdate = user.Customer?.Birthdate!;
            }
            
            return userInfo;
        }
    }
}
