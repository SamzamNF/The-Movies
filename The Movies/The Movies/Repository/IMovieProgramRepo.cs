using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Movies.Model;

namespace The_Movies.Repository
{
    interface IMovieProgramRepo
    {
        List<MovieProgram> GetAll();
        void Add(MovieProgram movieProgram);
        void Delete(MovieProgram movieProgram);
    }
}
