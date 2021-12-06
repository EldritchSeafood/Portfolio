using System;

namespace Assi3 {
    interface AbstractServer {
        bool AcceptQuery(Query query);
        int Execute(Route route);
        string GetRequestInfo();
        void SetRequest(Request request);
        bool IsAvailable();
    }
}
