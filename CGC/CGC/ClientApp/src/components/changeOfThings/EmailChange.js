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
        const receiver = {
            user: {
                password: this.refs.password.value,
                email: this.refs.newEmail.value,
                company: sessionStorage.getItem('company'),
                login: sessionStorage.getItem('login')
            }
        }
                //Przekazujesz User już i nowy email
                fetch(`api/Users/Change_Email`, {
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
                        if (json[0].error_Messege === "Email_taken") {
                            alert("Email is already taken!")
                        }
                        else {
                            sessionStorage.setItem('email', json[0].email)
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
                <div className="EmailChange">
                    <Sidebar />

                    <div className="title">
                        <h1 className="titletext">Change e-mail</h1>
                    </div>
                    <form>
                        <div className="EmailChange_c">
                            <div className="form-group">
                                <label>Your password:</label>
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
                                <label>New e-mail:</label>
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
                                <button type="button" className="success_change_own_em" onClick={this.emailChange}>Change e-mail</button>

                                <button type="button" className="danger_change_own_em" onClick={this.cancelChanging}>Cancel</button>

                            </div>


                        </div>


                    </form>
                </div>


               );
        }
    }
}