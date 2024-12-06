using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Repository.Data
{
    public static class CustomDBFunctions
    {
        [DbFunction("ConvertToInt", "dbo")]
        public static int ConvertToInt(string value) => throw new NotSupportedException();
    }
}
