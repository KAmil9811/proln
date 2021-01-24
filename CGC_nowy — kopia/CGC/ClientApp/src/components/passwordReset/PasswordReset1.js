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
            alert("Podaj adres email")
        }
        else {
            const user = {
                email: this.refs.email.value
            }
            fetch(`api/Users/Reset_Password_Code`, {
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
                    if (json[0].error_Messege === "Wrong_login_or_email") {
                alert("Podany email nie znajduje się w bazie danych")
            }
                    else {
                        alert("Wysłano kod odzyskiwania na email")
                        this.props.history.push('/reset_password2')
                    }
        })
        }
    }

    cancelPasswordReset1 = (event) => {
        this.props.history.push('/')
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
                <div className="resetpassword1">
                    <form>
                        <h2>Resetowanie hasła</h2>
                        <h3>  </h3>
                        <h3>  </h3>
                        <div className="form-group">
                            <label>e-mail:</label>
                            <input
                                type="email"
                                name="Email"
                                className="form-control"
                                id="inputEmail"
                                placeholder="Podaj email powiązany z kontem"
                                ref="email"
                            />
                        </div>
                        <div className="reset">
                            <button type="reset" className="danger_reset1" onClick={this.cancelPasswordReset1}>Anuluj</button>
                            <button type="submit" className="success_reset1" onClick={this.handlePasswordReset1} >Kontynuuj</button>

                        </div>

                    </form>
                </div>
            );
        }
    }
}