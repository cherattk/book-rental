$(document).ready(function () {

});

function searchMembre(form) {
	try {
		var url = "/Admin/ChercherMembre?nom=" + form.elements['nom'].value;
		$.ajax(url, function (data) {
			console.log(data);
		});

	} catch (e) {
		console.error(e);
	}
}

function searchLivre(form) {
	try {
		var url = "/Admin/ChercherLivre?titre=" + form.elements['titre'].value;
		$.ajax(url, function (data) {
			console.log(data);
		});

	} catch (e) {
		console.error(e);
	}
}

function messageInvitationConnection(admin) {
	var msg = "Vous devez vous connecter pour pouvoir reserver un livre";

	if (admin) {
		msg = "Vous etre connecté comme Administrateur Vous devez avoir un compte de type MEMBRE pour pouvoir réserver un livre";
	}

	document.getElementById('modal-alert-message').innerText = msg;
	var modalAlert = new bootstrap.Modal(document.getElementById('ModalAlert'));
	modalAlert.show();
}