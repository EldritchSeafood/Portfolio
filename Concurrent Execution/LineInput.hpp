/*
* Author: Dylan McNair
* Date: 2021-09-24
*/

#include <string>
#include <vector>

using namespace std;

class LineInput {
public:
    int launchGroup;                // The sequence to launch the programs in
    string programName;             // The name/path of the program to launch
    string cmdLnParameters;         // the parameters to be passed to the application via the command line

    LineInput(int launchGroup, string programName, string cmdLnParameters);
};