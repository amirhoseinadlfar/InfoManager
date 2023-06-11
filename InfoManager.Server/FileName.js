function SignUp(name, username, password) {
    const xhttp = new XMLHttpRequest();
    xhttp.open("POST", "/user/signup");
    const request = { name, username, password };
    xhttp.send(request);
}