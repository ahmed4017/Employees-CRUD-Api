﻿namespace Employees.Dtos
{
    public class EmployeeQueryParametersDto
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
