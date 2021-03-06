﻿using AutoMapper;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;
using Kbalan.TouchType.Data.Contexts;
using Kbalan.TouchType.Data.Models;
using Kbalan.TouchType.Logic.Dto;
using Kbalan.TouchType.Logic.Validators;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kbalan.TouchType.Logic.Services
{
    public class UploadService : IUploadService
    {
        private const int TenMegaBytes = 10 * 1024 * 1024;
        public UploadService()
        {
    
        }


        public async Task<Result<String>> UploadAsync(HttpPostedFile file, string userId)
        {
            
            try
            {

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(HttpContext.Current.Server.MapPath("~/"), folderName);
                if(file.ContentLength > TenMegaBytes )
                {
                    return Result.Failure<String>("File is more than 10mb");
                }
                var fileName =  userId + Path.GetExtension(file.FileName);
                if (file.ContentLength > 0)
                {

                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);


                         file.SaveAs(fullPath);
                    

                    return Result.Success( dbPath );
                }
                else
                {
                    return Result.Failure<String>("File is empty");
                }
            }
            catch (Exception ex)
            {
                return Result.Failure<String>(ex.Message);
            }
        }
    }
}
