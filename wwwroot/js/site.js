function countTime(count, href) {
	hours = Math.floor(count / 3600);
	minutes = Math.floor((count - hours * 3600) / 60);
	seconds = count - minutes * 60 - hours * 3600;
	if (hours < 10){ hours = "0" + hours; }
	if (minutes < 10){ minutes = "0" + minutes; }
	if (seconds < 10){ seconds = "0" + seconds; }
	if (count > 0){
		count--;
		document.getElementById("clock").innerHTML = hours + ":" + minutes + ":" + seconds;
		setTimeout("countTime("+count+")", 1000);
	} else {
		document.getElementById("clock").innerHTML = "Zakończono";
		location.href = href;
	}
}

function progress(time, w_s, w_e) {
	time++;
	timming = Math.floor((time - w_s) / (w_e - w_s) * 100);
	if (timming < 100) {
		document.getElementById("progressbar").style.width = timming+"%";
		setTimeout("progress("+time+", "+w_s+", "+w_e+")", 1000);
	} else {
		document.getElementById("progressbar").style.width = "100%";
	}
}