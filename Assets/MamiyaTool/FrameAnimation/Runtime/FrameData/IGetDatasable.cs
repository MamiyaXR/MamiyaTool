using System.Collections.Generic;

namespace MamiyaTool {
    public interface IGetDatasable<T> where T : FrameDataBase{
        List<T> Datas { get; }
    }
}