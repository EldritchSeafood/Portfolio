/*
* Author: Dylan McNair
* Date: 2021-09-24
*/

#include <fstream>
#include <iterator>
#include <map>
#include "Processor.cpp"


const string WHITESPACE = " \n\r\t\f\v";

string ltrim(const string& s) {
    size_t start = s.find_first_not_of(WHITESPACE);
    return (start == string::npos) ? "" : s.substr(start);
}

string rtrim(const string& s) {
    size_t end = s.find_last_not_of(WHITESPACE);
    return (end == string::npos) ? "" : s.substr(0, end + 1);
}

string trim(const string& s) {
    return rtrim(ltrim(s));
}

/// <summary>
/// Accepts a string with the name of a txt file and processes each line into a vector of strings.
/// </summary>
/// <returns>A vector containing each line of the provided txt file.</returns>
vector<string> readFileToVector(const string& filename) {
    // open the provided txt file
    ifstream inputFile;
    inputFile.open(filename);
    // read each line into a vector of strings
    vector<string> lines;
    string line;
    while (getline(inputFile, line)) {
        if (line != "") {
            lines.push_back(line);
        }
    }
    return lines;
}

Processor Processor::pro_Instance;

int main(int argc, char* argv[]) {
    if (argc > 2) { // too many parameters provided
        cout << "Too many parameters provided, please try again with only 1 file.";
    }
    else if (argc < 2) { // no parameters provided
        cout << "No parameters provided, please try again with a formatted ASCII text file.";
    }
    else { // one parameter provided (what we're looking for)
     // call a method to turn the txt file into a vector of lines
        vector<string> inputLines = readFileToVector(argv[1]);

        // go through each line and read them into a LineInput object then put them into a map based on their launch group
        vector<string>::iterator ptr;
        map<int, vector<LineInput>> launchGroups;
        int launchGroup;
        string programName;
        string cmdLnParameters = "";
        vector<string> results;

        for (ptr = inputLines.begin(); ptr < inputLines.end(); ptr++) {
            //cout << *ptr << "\n"; //temporarily printing stuff to see if working
            stringstream ss(*ptr);
            results = vector<string>();
            cmdLnParameters = "";

            // seperate the sections by commas
            while (ss.good()) {
                string substr;
                getline(ss, substr, ',');
                    results.push_back(substr);
            }

            // get rid of leading and lagging whitespace and assign to parameters
            launchGroup = stoi(trim(results[0]));
            programName = trim(results[1]);
            //uncomment to see it file reading
            // cout << to_string(launchGroup) << " " << programName << " ";

            // seperate the parameters by whitespace
            stringstream ssCmdLn(trim(results[2]));
            while (ssCmdLn.good()) {
                string substr;
                getline(ssCmdLn, substr, ' ');
                cmdLnParameters += substr + " ";
                // uncomment to see it file reading
                // cout << " " << substr; 
            }
            // put results into a LineInput object
            LineInput newInput = LineInput(launchGroup, programName, cmdLnParameters);
            // put the LineInput into a map based on its launchGroup
            // check current map to see if launch group is already there
            if (launchGroups.find(newInput.launchGroup) == launchGroups.end()) {
                // not found, create new entry in the map
                vector<LineInput> newLaunchGroup = vector<LineInput>();
                newLaunchGroup.push_back(newInput);
                // uncomment to see formatted entry data
                // cout << "New Launch Group: " << to_string(newInput.launchGroup) << " " << newInput.programName << " " << newInput.cmdLnParameters << endl;
                launchGroups.insert(pair<int, vector<LineInput>>(newInput.launchGroup, newLaunchGroup));
            }
            else {
                // found, insert new LineInput into entry with matching launch group
                // uncomment to see formatted entry data
                // cout << "Adding to Launch Group: " << to_string(newInput.launchGroup) << " " << newInput.programName << " " << newInput.cmdLnParameters << endl;
                launchGroups[newInput.launchGroup].push_back(newInput);
            }
        }

        // go through each launch group and execute the concurrently
        map<int, vector<LineInput>>::iterator groupPtr;
        for (groupPtr = launchGroups.begin(); groupPtr != launchGroups.end(); groupPtr++) {
            Processor::ExecuteLaunchGroup(groupPtr->second);
        }

        // call the Processor's print function to output the info to the console
        Processor::Print();
    }
}