using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Task6.Extensions;
using Task6.Models;

namespace Task6.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private enum ErrorType { AddChar, RemoveChar, SwapChars }

        public UserController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        [Route("api/users")]
        public List<User> GetAllUsers(int seed, string? culture, double? occurance)
        {
            List<User> users;

            if (culture != null)
            {
                users = GetUsersFromFile(culture, seed);
                
                if (occurance != null)
                {
                    users = GetCorruptedList((double)occurance, users, culture);
                }
            }
            else
            {
                users = GetUsersFromFile(String.Empty, seed);
                
                if (occurance != null)
                {
                    users = GetCorruptedList((double)occurance, users);
                }
            }

            return users;
        }

        private List<User> GetUsersFromFile(string key, int seed)
        {
            string path = Path.Combine(_env.ContentRootPath, @"Resources\Files\");
            List<User> users;

            if (!String.IsNullOrWhiteSpace(key))
            {
                users = JsonConvert.DeserializeObject<List<User>>(System.IO.File.ReadAllText(path + $"{key}.json"));
            }
            else
            {
                users = JsonConvert.DeserializeObject<List<User>>(System.IO.File.ReadAllText(path + "ru.json"));
            }

            if (seed < users.Count)
            {
                return users.Skip(seed).ToList();
            }

            return users;
        }

        private List<User> GetCorruptedList(double occurance, List<User> users, string culture = "ru")
        {
            var random = new Random();

            if (occurance > 0)
            {
                if (occurance % 1 != 0)
                {
                    if (occurance < 1)
                    {
                        int step = (int)(1 / occurance);

                        for (int i = 0; i < users.Count; i += step)
                        {
                            var user = users[i];
                            CorruptUser(random, user, culture);
                        }
                    }
                    else
                    {
                        double secondPart = occurance - (int)occurance;
                        int step = (int)(1 / secondPart);

                        foreach (var user in users)
                        {
                            for (int i = 0; i < occurance; i++)
                            {
                                CorruptUser(random, user, culture);
                            }
                        }

                        for (int i = 0; i < users.Count; i += step)
                        {
                            var user = users[i];
                            CorruptUser(random, user, culture);
                        }
                    }
                }
                else
                {
                    foreach (var user in users)
                    {
                        for (int i = 0; i < occurance; i++)
                        {
                            CorruptUser(random, user, culture);
                        }
                    }
                }
            } 

            return users;
        }

        private void CorruptUser(Random random, User user, string culture)
        {
            user.UniqueId = GenerateError((ErrorType)random.Next(0, 3), user.UniqueId, random.Next(0, user.UniqueId.Length), random.Next(0, user.UniqueId.Length), culture);
            user.Name = GenerateError((ErrorType)random.Next(0, 3), user.Name, random.Next(0, user.Name.Length), random.Next(0, user.Name.Length), culture);
            user.Address = GenerateError((ErrorType)random.Next(0, 3), user.Address, random.Next(0, user.Address.Length), random.Next(0, user.Address.Length), culture);
            user.PhoneNumber = GenerateError((ErrorType)random.Next(0, 3), user.PhoneNumber, random.Next(0, user.PhoneNumber.Length), random.Next(0, user.PhoneNumber.Length), culture);
        }

        private string GenerateError(ErrorType errorType, string corruptedStr, int position, int position2, string culture)
        {
            switch (errorType)
            {
                case ErrorType.AddChar:
                    corruptedStr = corruptedStr.AddChar(position, culture);
                    break;
                case ErrorType.RemoveChar:
                    corruptedStr = corruptedStr.RemoveChar(position);
                    break;
                case ErrorType.SwapChars:
                    corruptedStr = corruptedStr.SwapChars(position, position2);
                    break;
            }

            return corruptedStr;
        }
    }
}
