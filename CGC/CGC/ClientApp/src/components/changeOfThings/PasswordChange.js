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
                company: sessionStorage.getItem('company'),
            },
            new_password: this.refs.newPassword.value,


            /*oldPassword2: this.refs.oldPassword2.value,*/

            /* newPassword2: this.refs.newPassword2.value*/
        }
        //Przekazujesz User nowe hasło i nowe hasło (do potwierdzenia)
        fetch(`api/Users/Change_Password`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        })

            .then(res => res.json())
            .then(json => {
                console.log(json)
                return (json)
            })

            .then(json => {
                if (json[0].error_Messege === "Incorrect login or password") {
                    alert("Wrong old password!")
                }
                else if (json[0].error_Messege === "Incorrect password") {
                    alert("Wrong new password!")
                }
                else {
                    sessionStorage.setItem('password', json[0].password)
                    this.props.history.push('/userpanel')
                }
            })
    }

    cancelChanging = (event) => {
        this.props.history.push('/userpanel')
    }
    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }

    render() {
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePageFail">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else {
            return (
                <div className="PasswordChange">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Change password</h1>
                    </div>
                    <form>
                        <div className="PasswordChange_c">

                            <div className="form-group">

                                <label>Enter old password:</label>
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
                                <label>Enter new password:</label>
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

                                <button type="button" className="success_change_own_pass" onClick={this.passwordChange}>Change password</button>

                                <button type="button" className="danger_change_own_pass" onClick={this.cancelChanging}>Cancel</button>

                            </div>


                        </div>
                    </form>
                </div>


                
                );
        }
    }
}