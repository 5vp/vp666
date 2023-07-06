var MyPlugin = {
	StringReturnValueFunction: function()
	{
		var returnStr = window.location.search;
		var buffer = _malloc(lengthBytesUTF8(returnStr) + 1);
		stringToUTF8(returnStr,buffer,lengthBytesUTF8(returnStr) + 1);
		return buffer;
	}
};
mergeInto(LibraryManager.library,MyPlugin);