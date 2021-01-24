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
                console.log(json)
                return (json)
                
                
            })
            .then(json => {
                if (json[0].error_Messege === "Wrong_code" || this.refs.code.value === "") {
                    alert("Niepoprawny kod")
                } else {
                    alert('Wysłano nowe hasło na maila')
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
                <form>
                    <h2>Resetowanie hasła</h2>


                    <div className="form-group">
                        <label>Kod werfikacyjny</label>
                        <input
                            type="text"
                            name="Code"
                            className="form-control"
                            id="inputCode"
                            placeholder="Podaj kod"
                            ref="code"
                        />
                    </div>





                    <div className="form-group">
                        <button type="button" className="danger_reset2" onClick={this.cancelResetPassword2}>Anuluj</button>
                        <button type="button" className=" success_reset2" onClick={this.handlePasswordReset2}>Dalej</button>
                        
                    </div>

                </form>
            </div>
        );
    }
}