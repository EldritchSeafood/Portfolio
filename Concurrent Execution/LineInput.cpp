#include "LineInput.hpp"

LineInput::LineInput(int launchGroup, string programName, string cmdLnParameters) {
	this->launchGroup = launchGroup;
	this->programName = programName;
	this->cmdLnParameters = cmdLnParameters;
}