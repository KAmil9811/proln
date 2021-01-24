import React, { Component } from "react";
import './EmailChange.css';
import Sidebar from '../Sidebar';

export class EmailChange extends Component {
    displayName = EmailChange.name;
    constructor(props) {
        super(props);
    }

    emailChange = (event) => {
        event.preventDefault();
        const user = {
            password: this.refs.password.value,
            email: this.refs.newEmail.value,
            login: sessionStorage.getItem('login')
        }
            if (user.password === sessionStorage.getItem('password')) {
                //Przekazujesz User już i nowy email
                fetch(`api/Users/Change_Email`, {
                    method: "post",
                    body: JSON.stringify(user),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(res => res.json())
                    .then(json => {
                        console.log(json)
                        return (json)
                    })
                    .then(json => {
                        if (json[0].error_Messege === "Email_taken") {
                            alert("Email jest już zajęty!")
                        }
                        else {
                            sessionStorage.setItem('email', json[0].email)
                            this.props.history.push('/userpanel')
                        }
                    })
            }
            else {
                alert("Podano błędne hasło!")
            }
        }
    

    cancelChanging = (event) => {
        this.props.history.push('/userpanel')
    }

    render() {
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Zaloguj się, aby usyskać dostęp!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Logowanie</button>
                </div>
            );
        }
        else {
            return (
                <div className="Changeemail">
                    <Sidebar />
                    <div className="ChangeEmail">
                        <form>
                            <div className="form-group">
                                <h2>Zmiana e-maila</h2>
                                <label>Podaj hasło:</label>
                                <input
                                    type="password"
                                    name="Password"
                                    className="form-control"
                                    id="inputPassword"
                                    placeholder="*********"
                                    ref="password"
                                />
                            </div>
                            <div className="form-group">
                                <label>Podaj nowy email:</label>
                                <input
                                    type="email"
                                    name="newEmail"
                                    className="form-control"
                                    id="inputNewEmail"
                                    placeholder="smapleemail@domain.com"
                                    ref="newEmail"
                                />
                            </div>
                            <div className="form-group">
                                <button type="button" className="danger_change_own_em" onClick={this.cancelChanging}>Anuluj</button>
                                <button type="button" className="success_change_own_em" onClick={this.emailChange}>Zmiań email</button>

                            </div>

                        </form>
                    </div>
                </div>
            );
        }
    }
}