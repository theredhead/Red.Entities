(function(w, d){

	var socket = new WebSocket('ws://127.0.0.1:8181');

	socket.onopen = function(e) {
		console.log('WebSocket opened: ',e);
		var body = document.getElementsByTagName('body')[0];

		var tb = document.createElement('textarea');
		tb.setAttribute('rows', '5');
		tb.setAttribute('cols', '40');

		var button = document.createElement('button');
		button.appendChild(document.createTextNode("Send Message"));
		button.addEventListener('click', function() {
			socket.send(tb.value);
		});

		body.appendChild(tb);
		body.appendChild(button);
	};

	// Log errors
	socket.onerror = function (error) {
	  console.error('WebSocket Error ', error);
	};

	// Log messages from the server
	socket.onmessage = function (e) {
	  console.log('WebSocket message: ', e.data);
	};


}(window, document));