import React, { Component } from "react";
import Select from 'react-select';
import './AddAcount.css';
import Sidebar from '../Sidebar';


export class AddAcount extends Component{
    displayName = AddAcount.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            magazineManagement: false,
            machineManagement: false,
            orderManagement: false,
            cutManagement: false,
            user:false,
        }
    }



    handleAddAcount = (event) => {
        event.preventDefault();
        if (this.state.value === "user") {
            this.setState({ user: true})
        }
        const receiver = {
            admin: {
                login: sessionStorage.getItem('login')
            },
            user: {
                login: this.refs.login.value,
                password: this.refs.password.value,
                email: this.refs.email.value,
                name: this.refs.name.value,
                surname: this.refs.surname.value,
                
                Magazine_management: this.state.magazineManagement,
                Machine_management: this.state.machineManagement,
                Order_management: this.state.orderManagement,
                Cut_management: this.state.cutManagement,
                    
            },
            perm: this.state.value,
        }
        const admin = {
            login: sessionStorage.getItem('login')
        }
        
        fetch(`api/Users/Add_User_Admin`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                
                
                return (json);
            })
            .then(json => {
                const access2 = json[0].error_Messege
               
                if (access2 == null) {
                    alert("You added user")
                    this.props.history.push('/controlpaneladmin')
                }
                else if (access2 == "Wrong_passowrd") {
                    alert("The password must contain upper and lower case letters, number and must be at least 8 in length!")
                }
                else if (access2 == "Wrong_login") {  
                    alert("Login may only contain characters of the English alphabet!")
                }
                else if (access2 == "Login_taken") {
                    alert("Login is taken!")
                }
                else if (access2 == "Email_taken") {
                    alert("Email is taken!")
                }
                else if (access2 == "Wrong_email") {
                    alert("Wrong email!")
                }
                else {
                    alert("Something went wrong :(")
                }
            })
    }

    cancelAdding = (event) => {
        this.props.history.push('/controlpaneladmin')
    }
    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }


    permRender() {
        if (sessionStorage.getItem('manager') === 'true') {
            return (
                <div className="form-group">
                    <label>Permission:</label><br />
                    <select onChange={(e) => {
                        this.setState({ value: e.target.value });
                       
                    }} >
                        <option value="user">Employee</option>
                        <option value="admin">Admin</option>
                            <option value="superAdmin">Super admin</option>
                      
                    
                        </select><br />
                        <div className="uprawnienia">
                        <input className="check" type="checkbox" onChange={(e) => this.setState({ magazineManagement: !this.state.magazineManagement })} />   Magazine management <br />
                        <input className="check" type="checkbox" onChange={(e) => this.setState({ machineManagement: !this.state.machineManagement })} />  Machine management <br />
                        <input className="check" type="checkbox" onChange={(e) => this.setState({ orderManagement: !this.state.orderManagement })} />   Order management <br />
                        <input className="check" type="checkbox" onChange={(e) => this.setState({ cutManagement: !this.state.cutManagement })} />   Cut management <br />
                        </div>
                </div>
            )
        }
        else if (sessionStorage.getItem('superAdmin') === 'true') {
            return (
                <div className="form-group">
                    <label>Permission:</label><br />
                    <select onChange={(e) => {
                        this.setState({ value: e.target.value });
                        
                    }} >
                        <option value="user">Employee</option>
                        <option value="admin">Admin</option>
                    </select><br />
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ magazineManagement: !this.state.magazineManagement })} />   Magazine management<br />
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ machineManagement: !this.state.machineManagement })} />   Machine management<br />
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ orderManagement: !this.state.orderManagement })} />   Order management<br />
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ cutManagement: !this.state.cutManagement })} />   Cut management <br />
                </div>
            )
        }
        else {
            return (
                <div className="form-group">
                    <label>Permission:</label><br />
                    <select onChange={(e) => {
                        this.setState({ value: e.target.value });
                       
                    }} >
                        <option value="user">Pracownik</option>
                    </select><br/>
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ magazineManagement: !this.state.magazineManagement })} />   Magazine management<br />
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ machineManagement: !this.state.machineManagement })} />   Machine management<br />
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ orderManagement: !this.state.orderManagement })} />   Order management<br />
                    <input className="check" type="checkbox" onChange={(e) => this.setState({ cutManagement: !this.state.cutManagement })} />   Cut management<br />
                </div>
            )
        }
    }
    

    render() {
        var perm = this.permRender()
        if ((sessionStorage.getItem('valid') === '') && (sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') ) {
            return (
                <div className="HomePageFail">
                    <h1>Log in or check if you have perrmission to this panel</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else {
            return (

                <div className="AddAcount">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Add user</h1>
                    </div>
                    <form>
                        <div className="AddAcount_c">

                            <div className="form-group">

                                <label>Email:</label>
                                <input
                                    type="email"
                                    name="Email"
                                    className="form-control"
                                    id="inputEmail"
                                    placeholder="Enter email"
                                    ref="email"
                                />
                            </div>
                            <div className="form-group">
                                <label>Login:</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputLogin"
                                    placeholder="Enter login"
                                    ref="login"
                                />
                            </div>
                            <div className="form-group">
                                <label>Password:</label>
                                <input
                                    type="password"
                                    className="form-control"
                                    id="inputPassword"
                                    placeholder="Enter password"
                                    ref="password"
                                />
                            </div>
                            <div className="form-group">
                                <label>Name:</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputFirstName"
                                    placeholder="Enter name"
                                    ref="name"
                                />
                            </div>
                            <div className="form-group">
                                <label>Surname:</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputSecondName"
                                    placeholder="Enter surname"
                                    ref="surname"
                                />
                            </div>
                            {perm}

                            <div className="form-group">
                                <button type="button" className="success_n_add_a" onClick={this.handleAddAcount}>Add user</button>

                                <button type="button" className="danger_n_add_a" onClick={this.cancelAdding}>Cancel</button>

                            </div>


                        </div>
                    </form>

                </div>

                );
        }
    }
}