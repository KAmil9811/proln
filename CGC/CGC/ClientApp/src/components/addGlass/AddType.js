import React, { Component } from "react";
import './AddType.css';
import Sidebar from '../Sidebar';


export class AddType extends Component {
    displayName = AddType.name;
    constructor(props) {
        super(props);
    }

    handleAddType = (event) => {
        event.preventDefault();
        const receiver = {
            type: this.refs.type.value,
            user: {
                company: sessionStorage.getItem('company'),
                login: sessionStorage.getItem('login')
            }
        }

        fetch(`api/Magazine/Add_type_Admin`, {
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
                if (json[0] === 'Type_alredy_exist') {
                    alert("Type already exist")
                }
                else {
                    alert("You added nwe glass type")
                    this.props.history.push('/glassatibutes')
                }
            })
        console.log(receiver)
    }

    cancelAddType = (event) => {
        this.props.history.push('/glassatibutes')
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
        else if (sessionStorage.getItem('magazineManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true'){
            return (
                <div className="AddType">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Add glass type</h1>
                    </div>
                    <form>
                        <div className="AddType_c">

                            <div className="form-group">

                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputType"
                                    placeholder="Enter glass type"
                                    ref="type"
                                />
                            </div>
                            <div className="form-group">
                                <button type="button" className="success_glass_type_add" onClick={this.handleAddType}>Add type</button>

                                <button type="button" className="danger_glass_type_add" onClick={this.cancelAddType}>Cancel</button>

                            </div>


                        </div>
                    </form>
                </div>
                
                );
        }
        else {
            return (
                <div className="HomePage">
                    <button type="button" className="danger_glass_add" onClick={this.cancelAddGlass}>Cancel</button>
                    <button type="button" className="success_glass_add" onClick={this.handleAddGlass}>Add glass</button>
                </div>
            );

        }
    }
}