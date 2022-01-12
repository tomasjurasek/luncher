var connection = new signalR.HubConnectionBuilder()
    .withUrl("/voteHub")
    .withAutomaticReconnect()
    .build();

connection.on("ReceiveVote", function (restaurantId) {
    const selector = `.${restaurantId} .votes`;
    var votes = document.querySelector(selector);
    var votesNumber = parseInt(votes.textContent);
    if (isNaN(votesNumber)) {
        votesNumber = 1;
    }
    else {
        votesNumber = votesNumber + 1;
    }

    votes.textContent = votesNumber;
});

connection.start().then(function () {
    console.log("connected");
}).catch(function (err) {
    console.log(err)
});

document.querySelectorAll('.vote-btn').forEach(voteBtn => {
    voteBtn.addEventListener('click', (event) => {
        setVote(event);
    });
});

function setVote(event) {
    var restaurantId = event.target.parentNode.className;
    $.ajax({
        type: 'POST',
        url: '/api/vote',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(
            {
                restaurantId: restaurantId,
                userId: getUserId()
            }),
        complete: function () {
            event.target.disabled = true;
        }
    });
}

window.addEventListener("load", function () {
    loadVotedRestaurants();
});

function loadVotedRestaurants() {
    $.ajax({
        type: 'GET',
        url: `/api/vote/${getUserId()}`,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        complete: function (restaurants) {
            restaurants.responseJSON.forEach(restaurant => {
                const selector = `.${restaurant} .vote-btn`;
                var voteBtn = document.querySelector(selector);
                voteBtn.disabled = true;
            });
        }
    });
}

function getUserId() {
    var userId = localStorage.getItem("luncher:userId");
    if (userId) {
        return userId;
    }

    userId = Math.random().toString(36);
    localStorage.setItem("luncher:userId", userId);

    return userId;
}
