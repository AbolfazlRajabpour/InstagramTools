using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InstagramTools
{
    public interface ICommand
    {
        Task Do();
    }
}
