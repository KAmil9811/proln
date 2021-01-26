import React, { Component } from "react";
import './PasswordReset.css'

export class PasswordReset1 extends Component {
    displayName = PasswordReset1.name;
    constructor(props) {
        super(props);
    }

    handlePasswordReset1 = (event) => {
        event.preventDefault();
        if (this.refs.email.value === "") {
            alert("Enter e-mail")
        }
        else {
            const receiver = {
                user: { email: this.refs.email.value }
               
            }
            fetch(`api/Users/Reset_Password_Code`, {
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
                    if (json[0].error_Messege === "Wrong_login_or_email") {
                alert("There is no such e-mail in data base")
            }
                    else {
                        alert("Password recovery code has been sended")
                        this.props.history.push('/reset_password2')
                    }
        })
        }
    }

    cancelPasswordReset1 = (event) => {
        this.props.history.push('/')
    }

    render() {
        
            return (
                <div className="ResetPassword">
                    <div className="tit_log">

                        <h1 className="tit_text">Reset password</h1>
                    </div>
                    < form>
                                <div className="ResetPassword_c">
                            
                        


                                            <div className="form-group">
                                                <label>e-mail:</label>
                                                <input
                                                    type="email"
                                                    name="Email"
                                                    className="form-control"
                                                    id="inputEmail"
                                                    placeholder="Enter email that is conected with your account"
                                                    ref="email"
                                                />
                                            </div>
                                            <div className="reset">
                                                <button type="reset" className="danger_reset1" onClick={this.cancelPasswordReset1}>Cancel</button>
                                <button type="submit" className="success_reset1" onClick={this.handlePasswordReset1} >Continue</button>

                                            </div>

                        

                             </div>
                    </ form>
                </div>
            );
       
    }
}