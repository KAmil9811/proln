import React, { Component } from "react";
import './PasswordReset2.css'

export class PasswordReset2 extends Component {
    displayName = PasswordReset2.name;
    constructor(props) {
        super(props);
    }

    handlePasswordReset2 = (event) => {
        event.preventDefault();
        const receiver = {
            user: {Reset_pass: this.refs.code.value}
            
        }
        fetch(`api/Users/Reset_Password_Pass`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })

            .then(res => res.json())
            .then(json => {
                
                return (json)
                
                
            })
            .then(json => {
                if (json[0].error_Messege === "Wrong_code" || this.refs.code.value === "") {
                    alert("Wrong code")
                } else {
                    alert('New password has been sended to your e-maila=')
                    this.props.history.push('/')
                }
                
            })
    }

    cancelResetPassword2 = (event) => {
        this.props.history.push('/')
    }

    render() {
        
            return (


                <div className="PasswordReset2">
                    <div className="tit_log">

                        <h1 className="tit_text">Reset password </h1>
                    </div>

                    <form>
                            <div className="PasswordReset2_c">
                            
                        


                                        <div className="form-group">
                                            <label>Verification code</label>
                                            <input
                                                type="text"
                                                name="Code"
                                                className="form-control"
                                                id="inputCode"
                                                placeholder="Enter code"
                                                ref="code"
                                            />
                                        </div>





                                        <div className="form-group">
                                            <button type="button" className="danger_reset2" onClick={this.cancelResetPassword2}>Cancel</button>
                                            <button type="button" className=" success_reset2" onClick={this.handlePasswordReset2}>Accept</button>

                                        </div>

                        

                        </div>

                    </form>

                </div>
            );
       
    }
}