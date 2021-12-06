using System;

namespace Assi3 {
    class Server : AbstractServer {
        private Request request;

        /// <summary>Uses the Visitor pattern to check if the server can accept a new request</summary>
        public bool AcceptQuery(Query query) {
            return query.CheckServer(this);
        } //end Accept()

        /// <summary>Returns a printable string of the server's request's path and payload</summary>
        public string GetRequestInfo() {
            return "Path:\t" + request.path + "\nInput:\t" + request.payload.ToString();
        } //end GetRequestInfo()

        /// <summary>Set the server's current request</summary>
        public void SetRequest(Request request) {
            this.request = request;
        } //end SetRequest()

        /// <summary>Returns whether the server can accept a new request</summary>
        public bool IsAvailable() {
            return request == null;
        } //endIsAvailable

        /// <summary>Executes the request currently on the server</summary>
        public int Execute(Route route) {
            return request.Execute(route);
        } //end Execute()
    } //end Server class
}
