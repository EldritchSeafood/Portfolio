/*
* Author: Dylan McNair
* Date: 2021-10-11
*/

#include <process.h>
#include <cstdlib>
#include <iomanip>
#include <Windows.h>
#include <iostream>
#include <sstream>
#include <locale>
#include <codecvt>
#include "LineInput.hpp"

class Processor {
public:
    Processor(const Processor&) = delete;

    static Processor& Get() {
        return pro_Instance;
    }

    /// <summary>Accepts a group of system commands with their parameters that are to be run concurrently</summary>
    /// <param name="inputs">A vector filled with the information of each system that is to be run in this group</param>
    static void ExecuteLaunchGroup(vector<LineInput> inputs) {
        pro_Instance.infoOutput += "\n\nGroup:" + to_string(inputs[0].launchGroup);

        // this vector will hold all the startup info for each process
        vector<STARTUPINFO> sinInfoVec = vector<STARTUPINFO>();
        // this vector will hold all the startup info for each process
        vector<PROCESS_INFORMATION> procInfoVec = vector<PROCESS_INFORMATION>();

        unsigned long const CP_MAX_COMMANDLINE = 32768;

        // go through each entry in the inputs vector and create a process for it
        vector<LineInput>::iterator linePtr;
        wstring_convert<codecvt_utf8_utf16<wchar_t>> converter;
        int lineCount = 0;
        for (linePtr = inputs.begin(); linePtr < inputs.end(); linePtr++) {
            wstring tmpApp = converter.from_bytes(linePtr->programName);
            //wstring tmpApp(*linePtr->programName.begin(), *linePtr->programName.end());
            wstring application = L"\"" + tmpApp + L"\"";
            //wstring params(*linePtr->cmdLnParameters.begin(), *linePtr->cmdLnParameters.end());
            wstring params = converter.from_bytes(linePtr->cmdLnParameters);

            //wstring application = L"\"Notepad\"";
            //wstring params = L"freak.txt";

            wstring command = application + L" " + params;

            STARTUPINFO sinfo{ 0 };				// initialize all fields to zero
            sinfo.cb = sizeof(STARTUPINFO);
            sinInfoVec.push_back(sinfo);

            PROCESS_INFORMATION pi{ 0 };        // initialize all fields to zero
            procInfoVec.push_back(pi);

            try {
                wchar_t* commandline = new wchar_t[CP_MAX_COMMANDLINE];
                wcsncpy_s(commandline, CP_MAX_COMMANDLINE, command.c_str(), command.size() + 1);
                auto res = CreateProcess(NULL, commandline, NULL, NULL, false, CREATE_NEW_CONSOLE, NULL, NULL, &sinInfoVec[lineCount], &procInfoVec[lineCount]);
                if (res == 0) {
                    cerr << "Error: " << GetLastError() << endl;
                }

                delete[]commandline;
            }
            catch (std::bad_alloc&) {
                wcerr << L"Insufficient memory to launch application.\n";
                return;
            }
            lineCount++;
        } // end inputs reading

        // allocate memory for HANDLE
        HANDLE* hProcesses = new HANDLE[procInfoVec.size()];
        for (int i = 0; i < procInfoVec.size(); i++) {
            hProcesses[i] = procInfoVec[i].hProcess;
        }

        DWORD exitCode = 42;
        if (WAIT_FAILED == WaitForMultipleObjects(inputs.size(), hProcesses, TRUE, INFINITE)) {
            cerr << "Failure waiting for processes to terminate" << endl;
        }
        /*if (WAIT_FAILED == WaitForSingleObject(pi.hProcess, INFINITE))
            cerr << "Failure waiting for process to terminate" << endl;*/


        int processNum = 0;
        // go through each process and get the information out of them before closing them
        vector<PROCESS_INFORMATION>::iterator proPtr;
        for (proPtr = procInfoVec.begin(); proPtr < procInfoVec.end(); proPtr++) {
            stringstream timeString;

            FILETIME createTime, exitTime, kernelTime, userTime;
            GetProcessTimes(proPtr->hProcess, &createTime, &exitTime, &kernelTime, &userTime);
            SYSTEMTIME kTime;
            FileTimeToSystemTime(&kernelTime, &kTime);
            timeString << endl << "K:" << setw(2) << setfill('0') << kTime.wMinute << ":" << setw(2) << setfill('0') << kTime.wSecond << "." << setw(3) << setfill('0') << kTime.wMilliseconds;
            SYSTEMTIME uTime;
            FileTimeToSystemTime(&userTime, &uTime);
            timeString << " U:" << setw(2) << setfill('0') << uTime.wMinute << ":" << setw(2) << setfill('0') << uTime.wSecond << "." << setw(3) << setfill('0') << uTime.wMilliseconds;
            GetExitCodeProcess(proPtr->hProcess, &exitCode);
            timeString << " E:" << exitCode;
            timeString << " G:" << to_string(inputs[processNum].launchGroup) << " "; // CHANGE 0 TO BE THE CURRENT PROCESSES inputs EQUIVALENT
            timeString << inputs[processNum].programName << " " << inputs[processNum].cmdLnParameters; // CHANGE 0 TO BE THE CURRENT PROCESSES inputs EQUIVALENT

            // add the stream to the infoOutput for later printing
            pro_Instance.infoOutput += timeString.str();

            // add the errors (if any) to the errorOutput for later printing
            if (exitCode != 0) {
                pro_Instance.errorOutput += "G#: "
                    + to_string(inputs[processNum].launchGroup)
                    + "\tCommand: " + inputs[processNum].programName
                    + "\tParams: " + inputs[processNum].cmdLnParameters
                    + "\n ---> Error = " + to_string(exitCode) + "\n";
            }

            CloseHandle(proPtr->hThread);
            CloseHandle(proPtr->hProcess);
            processNum++;
        }

        // delete handle to deallocate memory
        delete[] hProcesses;

    } // end ExecuteLaunchGroup()

    static void Print() {
        cout << pro_Instance.infoOutput << "\n\n\n" << pro_Instance.errorOutput;
    } //end Print()
private:
    Processor() {}

    static Processor pro_Instance;

    string infoOutput = "Launch Times";
    string errorOutput = "Errors\n";
};