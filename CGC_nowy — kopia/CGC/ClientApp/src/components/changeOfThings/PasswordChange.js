import React, { Component } from "react";
import './PasswordChange.css';
import Sidebar from '../Sidebar';

export class PasswordChange extends Component {
    displayName = PasswordChange.name;
    constructor(props) {
        super(props);
    }

    passwordChange = (event) => {
        event.preventDefault();
        const receiver = {
            user: {
                login: sessionStorage.getItem('login'),
                password: this.refs.oldPassword.value,
            },
            new_password: this.refs.newPassword.value,


            /*oldPassword2: this.refs.oldPassword2.value,*/
            
           /* newPassword2: this.refs.newPassword2.value*/
        }
         
        if (receiver.user.password === this.refs.oldPassword2.value) {
            if (receiver.user.password === sessionStorage.getItem('password')) {
                if (receiver.new_password === this.refs.newPassword2.value) {

                    //Przekazujesz User nowe hasło i nowe hasło (do potwierdzenia)
                    fetch(`api/Users/Change_Password`, {
                        method: "post",
                        body: JSON.stringify(receiver),
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
                           if (json[0].error_Messege === "Wrong_old_password") {
                               alert("Podano błędne stare hasło!")
                            }
                           else {
                               sessionStorage.setItem('password', json[0].password)
                               this.props.history.push('/controlpanel')
                            }
                       })      
                }
                else {
                    alert("Nowe hasła nie są takie same!")
                }
            }
            else {
                alert("Stare hasło się nie zgadza!")
            }
        }
        else {
            alert("Wpisano dwa różne stare hasła!")
        }
    }

    cancelChanging = (event) => {
        this.props.history.push('/controlpanel')
    }

    render() {
        return (
            <div className="changepassword">
                <Sidebar />
                <div className="ChangePassword">
                    <form>
                        <div className="form-group">
                            <h2>Zmiana hasła</h2>
                            <label>Podaj stare hasło:</label>
                            <input
                                type="password"
                                name="Password"
                                className="form-control"
                                id="inputOldPassword"
                                placeholder="*********"
                                ref="oldPassword"
                            />
                        </div>
                        <div className="form-group">
                       
                            <label>Powtórz stare hasło:</label>
                            <input
                                type="password"
                                name="Password"
                                className="form-control"
                                id="inputOldPassword2"
                                placeholder="*********"
                                ref="oldPassword2"
                            />
                        </div>
                        <div className="form-group">
                            <label>Podaj nowe hasło:</label>
                            <input
                                type="password"
                                name="Password"
                                className="form-control"
                                id="inputNewPassword"
                                placeholder="*********"
                                ref="newPassword"
                            />
                        </div>
                        <div className="form-group">
                            <label>Podaj nowe hasło:</label>
                            <input
                                type="password"
                                name="Password"
                                className="form-control"
                                id="inputNewPassword2"
                                placeholder="*********"
                                ref="newPassword2"
                            />
                        </div>
                        <div className="form-group">
                            <button type="button" className="danger_change_own_pass" onClick={this.cancelChanging}>Anuluj</button>
                            <button type="button" className="success_change_own_pass" onClick={this.passwordChange}>Zmiań hasła</button>
                        
                        </div>

                    </form>
                </div>
            </div>
        );
    }
}