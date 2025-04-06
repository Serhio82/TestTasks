using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MyDataBaseService.Entities;
using System.ComponentModel.DataAnnotations;


namespace MyDataBaseService
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

           
            string connection = "DataSourse=D:\\MyDataBase.db";

            builder.Services.AddDbContext<MyDataBaseServiceContext>(options => options.UseSqlServer(connection));

            var app = builder.Build();

            
            app.UseSwagger();
            app.UseSwaggerUI();
            
            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Employee
            app.MapGet("/api/employees", async (MyDataBaseServiceContext db) => await db.Employees.ToListAsync());

            app.MapGet("/api/employees/{id:int}", async (int id, MyDataBaseServiceContext db) =>
            {
                // получаем пользователя по id
                Employee? employee = await db.Employees.FirstOrDefaultAsync(u => u.Id == id);

                // если не найден, отправляем статусный код и сообщение об ошибке
                if (employee == null) return Results.NotFound(new { message = "Пользователь не найден" });

                // если пользователь найден, отправляем его
                return Results.Json(employee);
            });

            app.MapDelete("/api/employees/{id:int}", async (int id, MyDataBaseServiceContext db) =>
            {
                // получаем пользователя по id
                Employee? employee = await db.Employees.FirstOrDefaultAsync(u => u.Id == id);

                // если не найден, отправляем статусный код и сообщение об ошибке
                if (employee == null) return Results.NotFound(new { message = "Пользователь не найден" });

                // если пользователь найден, удаляем его
                db.Employees.Remove(employee);
                await db.SaveChangesAsync();
                return Results.Json(employee);
            });

            app.MapPost("/api/employees", async (Employee employee, MyDataBaseServiceContext db) =>
            {
                // добавляем пользователя в массив
                await db.Employees.AddAsync(employee);
                await db.SaveChangesAsync();
                return employee;
            });

            app.MapPut("/api/employees", async (Employee employeeData, MyDataBaseServiceContext db) =>
            {
                // получаем пользователя по id
                var employee = await db.Employees.FirstOrDefaultAsync(u => u.Id == employeeData.Id);

                // если не найден, отправляем статусный код и сообщение об ошибке
                if (employee == null) return Results.NotFound(new { message = "Пользователь не найден" });

                // если пользователь найден, изменяем его данные и отправляем обратно клиенту
                employee.Name = employeeData.Name;
                await db.SaveChangesAsync();
                return Results.Json(employee);
            });

            // Post
            app.MapGet("/api/posts", async (MyDataBaseServiceContext db) => await db.Posts.ToListAsync());

            app.MapGet("/api/posts/{id:int}", async (int id, MyDataBaseServiceContext db) =>
            {
                // получаем пользователя по id
                Post? post = await db.Posts.FirstOrDefaultAsync(u => u.Id == id);

                // если не найден, отправляем статусный код и сообщение об ошибке
                if (post == null) return Results.NotFound(new { message = "Пользователь не найден" });

                // если пользователь найден, отправляем его
                return Results.Json(post);
            });

            app.MapDelete("/api/users/{id:int}", async (int id, MyDataBaseServiceContext db) =>
            {
                // получаем пользователя по id
                Post? post = await db.Posts.FirstOrDefaultAsync(u => u.Id == id);

                // если не найден, отправляем статусный код и сообщение об ошибке
                if (post == null) return Results.NotFound(new { message = "Пользователь не найден" });

                // если пользователь найден, удаляем его
                db.Posts.Remove(post);
                await db.SaveChangesAsync();
                return Results.Json(post);
            });

            app.MapPost("/api/users", async (Post post, MyDataBaseServiceContext db) =>
            {
                // добавляем пользователя в массив
                await db.Posts.AddAsync(post);
                await db.SaveChangesAsync();
                return post;
            });

            app.MapPut("/api/posts", async (Post postData, MyDataBaseServiceContext db) =>
            {
                // получаем пользователя по id
                var post = await db.Posts.FirstOrDefaultAsync(u => u.Id == postData.Id);

                // если не найден, отправляем статусный код и сообщение об ошибке
                if (post == null) return Results.NotFound(new { message = "Пользователь не найден" });

                // если пользователь найден, изменяем его данные и отправляем обратно клиенту
                
                post.Name = postData.Name;
                await db.SaveChangesAsync();
                return Results.Json(post);
            });

            // Department
            app.MapGet("/api/departments", async (MyDataBaseServiceContext db) => await db.Departments.ToListAsync());

            app.MapGet("/api/departments/{id:int}", async (int id, MyDataBaseServiceContext db) =>
            {
                // получаем пользователя по id
                Department? department = await db.Departments.FirstOrDefaultAsync(u => u.Id == id);

                // если не найден, отправляем статусный код и сообщение об ошибке
                if (department == null) return Results.NotFound(new { message = "Пользователь не найден" });

                // если пользователь найден, отправляем его
                return Results.Json(department);
            });

            app.MapDelete("/api/departments/{id:int}", async (int id, MyDataBaseServiceContext db) =>
            {
                // получаем пользователя по id
                Department? department = await db.Departments.FirstOrDefaultAsync(u => u.Id == id);

                // если не найден, отправляем статусный код и сообщение об ошибке
                if (department == null) return Results.NotFound(new { message = "Пользователь не найден" });

                // если пользователь найден, удаляем его
                db.Departments.Remove(department);
                await db.SaveChangesAsync();
                return Results.Json(department);
            });

            app.MapPost("/api/departments", async (Department department, MyDataBaseServiceContext db) =>
            {
                // добавляем пользователя в массив
                await db.Departments.AddAsync(department);
                await db.SaveChangesAsync();
                return department;
            });

            
            app.Run();
        }
    }
}
