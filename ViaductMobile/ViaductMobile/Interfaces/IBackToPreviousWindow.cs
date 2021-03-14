using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ViaductMobile.Interfaces
{
    public interface IBackToPreviousWindow
    {
        void BackViaAppButton(object sender, EventArgs e);
        void BackViaSystemButton();
    }
}
