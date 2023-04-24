﻿using MyHomework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHomework.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetUsersAsync();
        void ExportUsers(List<User> users);
    }
}
