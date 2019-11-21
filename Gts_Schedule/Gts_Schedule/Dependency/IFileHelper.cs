using System;
using System.Collections.Generic;
using System.Text;

namespace Gts_Schedule.Dependency
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }
}
