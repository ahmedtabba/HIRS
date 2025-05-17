//webkitURL is deprecated but nevertheless
//appNote.js
URL = window.URL || window.webkitURL;

var gumStream; 						//stream from getUserMedia()
var input;
var recorder;

//MediaStreamAudioSourceNode  we'll be recording
var encodingType; 					//holds selected encoding for resulting audio (file)
var encodeAfterRecord = true;       // when to encode

// shim for AudioContext when it's not avb. 
var AudioContext = window.AudioContext || window.webkitAudioContext;
var audioContext; //new audio context to help us record


var encodingTypeSelect = document.getElementById("encodingTypeSelect");
var recordButton = document.getElementsByClassName("recordButtonClass");
var recordReplyButton = document.getElementsByClassName("recordReplyButtonClass");
var sendpRecordButton = document.getElementById("SendRecordbtn");
var stopRecordButton = document.getElementById("StopRecordBtn");
var DeleteRecordButton = document.getElementById("DeleteRecordBtn");

var CloseModalButton = document.getElementById("CloseRecordModal");
var minutesLabel = document.getElementById("minutesrecord");
var secondsLabel = document.getElementById("secondsrecord");

var totalSeconds = 0;
//var stopButton = document.getElementById("stopButton");

//add events to those 2 buttons
sendpRecordButton.addEventListener("click", stopRecording, this);
stopRecordButton.addEventListener("click", stopRecording, this);
CloseModalButton.addEventListener("click", Close, this);
DeleteRecordButton.addEventListener("click", Close, this);
var Interval;

for (var i = 0; i < recordButton.length; i++) {
	recordButton[i].addEventListener("click", startRecording, this);
	//	recordButton[i].addEventListener("mouseup", stopRecording,this);
}
for (var i = 0; i < recordReplyButton.length; i++) {
	recordReplyButton[i].addEventListener("click", startReplyRecording, this);
	//recordReplyButton[i].addEventListener("mouseup", stopReplyRecording, this);
}
function Close(e) {
	IsModalClosePressed = true;
	isStop = true;

	var recordingsList = document.getElementById("recordingsList");
	recordingsList.innerHTML = "";
	$('#RecordModal').modal('hide');
}
function startRecording(e) {
	var modal = $("#RecordModal");
	var recordingsList = document.getElementById("recordingsList");
	recordingsList.innerHTML = "";
	//IsModalClosePressed = false;
	$('#StopRecordBtn').show();
	$('#DeleteRecordBtn').hide();
	setTimeout(function () {
		modal.modal("show");

	}, 800);

	if (!isRecordPressed) {
		isRecordPressed = true;
		IsReplayButton = false;
		console.log("startRecording() called");
		$(this).removeClass("notRec");
		$(this).addClass("Rec");
		var target = e.currentTarget;
		dataChannelId = target.dataset.channelid;
		/*
			Simple constraints object, for more advanced features see
			https://addpipe.com/blog/audio-constraints-getusermedia/
		*/

		var constraints = { audio: true, video: false }

		/*
			We're using the standard promise based getUserMedia() 
			https://developer.mozilla.org/en-US/docs/Web/API/MediaDevices/getUserMedia
		*/

		navigator.mediaDevices.getUserMedia(constraints).then(function (stream) {
			__log("getUserMedia() success, stream created, initializing WebAudioRecorder...");

			/*
				create an audio context after getUserMedia is called
				sampleRate might change after getUserMedia is called, like it does on macOS when recording through AirPods
				the sampleRate defaults to the one set in your OS for your playback device
	
			*/
			audioContext = new AudioContext();

			//update the format 
			document.getElementById("formats").innerHTML = "Format: 2 channel " + encodingTypeSelect.options[encodingTypeSelect.selectedIndex].value + " @ " + audioContext.sampleRate / 1000 + "kHz"

			//assign to gumStream for later use
			gumStream = stream;

			/* use the stream */
			input = audioContext.createMediaStreamSource(stream);

			//stop the input from playing back through the speakers
			//input.connect(audioContext.destination)

			//get the encoding 
			encodingType = encodingTypeSelect.options[encodingTypeSelect.selectedIndex].value;

			//disable the encoding selector
			encodingTypeSelect.disabled = true;
			recorder = new WebAudioRecorder(input, {
				workerDir: "../assets/js/", // must end with slash
				encoding: encodingType,
				numChannels: 2, //2 is the default, mp3 encoding supports only 2
				onEncoderLoading: function (recorder, encoding) {
					// show "loading encoder..." display
					__log("Loading " + encoding + " encoder...");
				},
				onEncoderLoaded: function (recorder, encoding) {
					// hide "loading encoder..." display
					__log(encoding + " encoder loaded");
				}
			});
			$(document).ready(function () {
				recorder.onComplete = function (recorder, blob) {
					__log("Encoding complete");
					createDownloadLink(blob, recorder.encoding);
					encodingTypeSelect.disabled = false;
					var myFile = blobToFile(blob, "my-file.wav");
					var formData = new FormData();
					formData.append("VoiceNote", myFile);
					if (!IsModalClosePressed) {
						if (!isStop) {
							if (!IsReplayButton) {
								
								UploadVoiceNote(formData);
							} else {
								UploadReplyVoiceNote(formData, parentId);
							}
						} else {
							BufferVoiceMessage = formData;
						}

					}




				}

			});

			recorder.setOptions({
				timeLimit: 120,
				encodeAfterRecord: encodeAfterRecord,
				ogg: { quality: 0.5 },
				mp3: { bitRate: 160 }
			});

			//start the recording process
			recorder.startRecording();

			__log("Recording started");
			Interval = setInterval(setTime, 1000);


		}).catch(function (err) {
			//enable the record button if getUSerMedia() fails
			IsModalClosePressed = true;
			isStop = true;
			recordButton.disabled = false;
			var recordingsList = document.getElementById("recordingsList");
			recordingsList.innerHTML = "";
			//stopButton.disabled = true;

		});

		//disable the record button
		//  recordButton.disabled = true;
		// stopButton.disabled = false;
	}
	else {

	}
}

function stopRecording(e) {
	
	var btnId = e.currentTarget.id;
	if (btnId == "StopRecordBtn") {
		isStop = true;
		$('#StopRecordBtn').hide();
		$('#DeleteRecordBtn').show();


		clearInterval(Interval);
	} else {
		isStop = false;
	}
	isRecordPressed = false;
	IsRecordReplyPressed = false;
	$(".recordButtonClass").removeClass("Rec");
	$(".recordButtonClass").addClass("notRec");
	$(".recordReplyButtonClass").removeClass("Rec");
	$(".recordReplyButtonClass").addClass("notRec");
	console.log("stopRecording() called");

	//stop microphone access
	gumStream.getAudioTracks()[0].stop();

	//disable the stop button
	//	stopButton.disabled = true;
	//recordButton.disabled = false;

	//tell the recorder to finish the recording (stop recording + encode the recorded audio)
	recorder.finishRecording();
	//$("#RecordModal").modal("hide");
	__log('Recording stopped');
	totalSeconds = 0;

	clearInterval(Interval);
	

	if (!IsModalClosePressed) {


		if (BufferVoiceMessage != null) {
			if (!IsReplayButton) {
				
				UploadVoiceNote(BufferVoiceMessage);
				BufferVoiceMessage = null;
			} else {
				UploadReplyVoiceNote(BufferVoiceMessage, parentId);
				BufferVoiceMessage = null;
			}
		}
	}
}


function stopRecordingForClose() {
	isRecordPressed = false;
	IsRecordReplyPressed = false;
	$(".recordButtonClass").removeClass("Rec");
	$(".recordButtonClass").addClass("notRec");
	$(".recordReplyButtonClass").removeClass("Rec");
	$(".recordReplyButtonClass").addClass("notRec");
	console.log("stopRecording() called");

	//stop microphone access
	gumStream.getAudioTracks()[0].stop();

	//disable the stop button
	//	stopButton.disabled = true;
	//recordButton.disabled = false;

	//tell the recorder to finish the recording (stop recording + encode the recorded audio)
	recorder.finishRecording();
	//$("#RecordModal").modal("hide");
	__log('Recording stopped');
	
	totalSeconds = 0;

	clearInterval(Interval);


}
function startReplyRecording(e) {
	if (!IsRecordReplyPressed) {
		var modal = $("#RecordModal");
		var recordingsList = document.getElementById("recordingsList");
		recordingsList.innerHTML = "";
		//IsModalClosePressed = false;
		$('#StopRecordBtn').show();
		$('#DeleteRecordBtn').hide();
		setTimeout(function () {
			modal.modal("show");

		}, 800);
		$(this).removeClass("notRec");
		$(this).addClass("Rec");
		IsRecordReplyPressed = true;
		var target = e.currentTarget;
		parentId = target.dataset.parentid;
		datachannelreply = target.dataset.channelid;
		ObjectType = target.dataset.objecttype;
		ObjectId = target.dataset.objectid;
		ObjectTag = target.dataset.objecttag;
		IsReplayButton = true;
		console.log("startRecording() called");

		/*
			Simple constraints object, for more advanced features see
			https://addpipe.com/blog/audio-constraints-getusermedia/
		*/

		var constraints = { audio: true, video: false }

		/*
			We're using the standard promise based getUserMedia() 
			https://developer.mozilla.org/en-US/docs/Web/API/MediaDevices/getUserMedia
		*/

		navigator.mediaDevices.getUserMedia(constraints).then(function (stream) {
			__log("getUserMedia() success, stream created, initializing WebAudioRecorder...");

			/*
				create an audio context after getUserMedia is called
				sampleRate might change after getUserMedia is called, like it does on macOS when recording through AirPods
				the sampleRate defaults to the one set in your OS for your playback device
	
			*/
			audioContext = new AudioContext();

			//update the format 
			document.getElementById("formats").innerHTML = "Format: 2 channel " + encodingTypeSelect.options[encodingTypeSelect.selectedIndex].value + " @ " + audioContext.sampleRate / 1000 + "kHz"

			//assign to gumStream for later use
			gumStream = stream;

			/* use the stream */
			input = audioContext.createMediaStreamSource(stream);

			//stop the input from playing back through the speakers
			//input.connect(audioContext.destination)

			//get the encoding 
			encodingType = encodingTypeSelect.options[encodingTypeSelect.selectedIndex].value;

			//disable the encoding selector
			encodingTypeSelect.disabled = true;
			recorder = new WebAudioRecorder(input, {
				workerDir: "../assets/js/", // must end with slash
				encoding: encodingType,
				numChannels: 2, //2 is the default, mp3 encoding supports only 2
				onEncoderLoading: function (recorder, encoding) {
					// show "loading encoder..." display
					__log("Loading " + encoding + " encoder...");
				},
				onEncoderLoaded: function (recorder, encoding) {
					// hide "loading encoder..." display
					__log(encoding + " encoder loaded");
				}
			});
			$(document).ready(function () {
				recorder.onComplete = function (recorder, blob) {
					__log("Encoding complete");
					createDownloadLink(blob, recorder.encoding);
					encodingTypeSelect.disabled = false;
					var myFile = blobToFile(blob, "my-file.wav");
					var formData = new FormData();
					formData.append("VoiceNote", myFile);
					if (!IsModalClosePressed) {
						if (!isStop) {
							if (!IsReplayButton) {
								
								UploadVoiceNote(formData);
							} else {
								UploadReplyVoiceNote(formData, parentId);
							}
						} else {
							BufferVoiceMessage = formData;
						}

					}




				}

			});

			recorder.setOptions({
				timeLimit: 120,
				encodeAfterRecord: encodeAfterRecord,
				ogg: { quality: 0.5 },
				mp3: { bitRate: 160 }
			});

			//start the recording process
			recorder.startRecording();

			__log("Recording started");
			Interval = setInterval(setTime, 1000);
		}).catch(function (err) {
			//enable the record button if getUSerMedia() fails
			recordButton.disabled = false;
			IsModalClosePressed = true;
			IsStop = true;
			var recordingsList = document.getElementById("recordingsList");
			recordingsList.innerHTML = "";
			//stopButton.disabled = true;

		});

		//disable the record button
		//  recordButton.disabled = true;
		// stopButton.disabled = false;
	}
	else {

	}
}

function stopReplyRecording(e) {
	var btnId = e.currentTarget.id;
	if (btnId == "StopRecordBtn") {
		isStop = true;
		$('#StopRecordBtn').hide();
		$('#DeleteRecordBtn').show();
		clearInterval(Interval);
	} else {
		isStop = false;
	}
	isRecordPressed = false;
	IsRecordReplyPressed = false;
	//$(".recordButtonClass").removeClass("Rec");
	//$(".recordButtonClass").addClass("notRec");
	//$(".recordReplyButtonClass").removeClass("Rec");
	//$(".recordReplyButtonClass").addClass("notRec");

	console.log("stopRecording() called");

	//stop microphone access
	gumStream.getAudioTracks()[0].stop();

	//disable the stop button
	//	stopButton.disabled = true;
	//recordButton.disabled = false;

	//tell the recorder to finish the recording (stop recording + encode the recorded audio)
	recorder.finishRecording();

	__log('Recording stopped');
	
	totalSeconds = 0;

	clearInterval(Interval);

}

function createDownloadLink(blob, encoding) {

	var url = URL.createObjectURL(blob);
	var au = document.createElement('audio');
	var li = document.createElement('li');
	var link = document.createElement('a');

	//add controls to the <audio> element
	au.controls = true;
	au.src = url;

	//link the a element to the blob
	link.href = url;
	link.download = new Date().toISOString() + '.' + encoding;
	link.innerHTML = link.download;

	//add the new audio and a elements to the li element
	li.appendChild(au);

	var recordingsList = document.getElementById("recordingsList");
	recordingsList.innerHTML = "";
	if (!IsStop && !IsModalClosePressed) {
		var isHidden = $('#StopRecordBtn').is(":hidden");
		if (isHidden) {
			recordingsList.appendChild(li);

		}
	}


}



//helper function
function __log(e, data) {
	log.innerHTML += "\n" + e + " " + (data || '');
}
function blobToFile(theBlob, fileName) {
	//A Blob() is almost a File() - it's just missing the two properties below which we will add
	theBlob.lastModifiedDate = new Date();
	theBlob.name = fileName;
	return theBlob;
}
function setTime() {
	++totalSeconds;
	secondsLabel.innerHTML = pad(totalSeconds % 60);
	minutesLabel.innerHTML = pad(parseInt(totalSeconds / 60));
}
function pad(val) {
	var valString = val + "";
	if (valString.length < 2) {
		return "0" + valString;
	}
	else {
		return valString;
	}
}