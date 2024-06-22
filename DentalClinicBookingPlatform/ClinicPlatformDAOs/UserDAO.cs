﻿using ClinicPlatformBusinessObject;
using ClinicPlatformDTOs.UserModels;
using ClinicPlatformDAOs.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace ClinicPlatformDAOs
{
    public class UserDAO : IFilterQuery<User>, IDisposable
    {
        private readonly DentalClinicPlatformContext _context;
        private bool disposedValue;

        public UserDAO()
        {
            _context = new DentalClinicPlatformContext();
        }

        public UserDAO(DentalClinicPlatformContext context)
        {
            _context = context;
        }

        public bool AddUser(User user)
        {
            _context.Add(user);
            this.SaveChanges();
            return true;
        }

        public User? GetUser(int id)
        {
            return _context.Users.Where(x => x.Id == id)
                .Include(x => x.Customers)
                .Include(x => x.Dentist)
                .FirstOrDefault();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users
                .Include(x => x.Customers)
                .Include(x => x.Dentist)
                .ToList();
        }

        public Customer? GetCustomerByCustomerId(int customerId)
        {
            return _context.Customers
                .Include(x => x.User)
                .Where(x => x.Id == customerId)
                .FirstOrDefault();
        }

        public Dentist? GetStaffByStaffId(int staffId)
        {
            return _context.Dentists
                .Include(x => x.User)
                .Where(x => x.UserId == staffId)
                .FirstOrDefault();
        }

        public bool UpdateUser(User user)
        {
            User? userInfo = GetUser(user.Id);

            if (userInfo != null)
            {
                _context.Users.Update(user);
                SaveChanges();

                return true;
            }

            return false;
        }

        public bool DeleteUser(int userId)
        {
            User? user = GetUser(userId);

            if (user != null)
            {
                _context.Users.Remove(user);
                this.SaveChanges();

                return true;
            }

            return false;
        }

        public void SaveChanges()
        {
            this._context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this._context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<User> Filter(Expression<Func<User, bool>> filter, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, string includeProperties = "", int? pageSize = null, int? pageIndex = null)
        {
            IQueryable<User> query = _context.Users;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Implementing pagination
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                // Ensure the pageIndex and pageSize are valid
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10; // Assuming a default pageSize of 10 if an invalid value is passed

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return query.ToList();
        }
    }
}