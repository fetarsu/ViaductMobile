using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ViaductMobile.Interfaces
{
    public interface IBackToPreviousWindow
    {
        Task BackViaAppButton();
        Task BackViaSystemButton();
    }
}
