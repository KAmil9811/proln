import React, { Component, useState } from "react";
import './AddGlass.css'
import Sidebar from '../Sidebar';
import ClipLoader from "react-spinners/ClipLoader";

export class AddGlass extends Component {
    displayName = AddGlass.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            value2: '',
            colors: [],
            type:  [],
            isLoading: false, 
        }
    }



    componentDidMount() {
        var table2 = [];
        var table3 = [];
        const receiver = {
            user: {
                company: sessionStorage.getItem('company'),
            }
        }
        fetch(`api/Magazine/Return_All_Colors`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
               
                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        color: json[i],
                    })
                };
                this.setState({
                    colors: table2
                
                });
            })
       
 fetch(`api/Magazine/Return_All_Type`, {
     method: "post",
     body: JSON.stringify(receiver),
     headers: {
         'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
         'Content-Type': 'application/json'
     }
        })
            .then(res => res.json())
            .then(json => {
                
                for (var i = 0; i < json.length; i++) {
                    table3.push({
                        type: json[i],
                    })
                };
                this.setState({
                    type: table3
                
                });
            })
    }
   

    handleAddGlass = (event) => {
        event.preventDefault();
        
        if (this.refs.height.value === "" || this.refs.width.value === "" || this.refs.length.value === "" || this.refs.owner.value === "" || this.refs.desk.value === "" || this.refs.color.value === "" || this.refs.type.value === "" || this.refs.amount.value === "" ) {
            alert("Fill the data")
        }
        else if (this.refs.length.value >= 5001 || this.refs.width.value >= 5001 || this.refs.amount.value >= 101 || this.refs.length.value <= 199 || this.refs.width.value <= 199) {
            alert("Limit exceeded")
            
        }
        else {
            this.setState({
            isLoading: true
            })
            const receiver = {
                glass: {
                    hight: this.refs.height.value,
                    width: this.refs.width.value,
                    length: this.refs.length.value, 
                    owner: this.refs.owner.value,
                    desk: this.refs.desk.value,
                    color: this.refs.color.value,
                    type: this.refs.type.value,
                    count: this.refs.amount.value,
                },
                user: {
                    company: sessionStorage.getItem('company'),
                    login: sessionStorage.getItem('login'),
                }
            }
            
            fetch(`api/Magazine/Add_Glass`, {
                method: "POST",
                headers: {
                    'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(receiver
                ),
            })

                .then(res => res.json())
                .then(json => {
                    return (json);
                    
                })
           
                .then(json => {
                    const glass2 = json[0].error_Messege
                    
                    if (glass2 == null) {
                        this.setState({
                            isLoading: false
                        })
                        alert("You added glass")
                        this.props.history.push('/glasswarehouse')
                    }
                    else if (glass2 == "User_no_permission") {
                        this.setState({
                            isLoading: false
                        })
                        alert("You have no perrmission to add glass");
                    }

                    

                })
            
        }
    }

    cancelAddGlass = (event) => {
        this.props.history.push('/glasswarehouse')
       
    }

    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }

    colorsSelector = (event) => {
        var tab = []
        for (var i = 0; i < this.state.colors.length; i++) {

            tab.push( < option value = { this.state.colors[i].color } > { this.state.colors[i].color }</option >)


        }
        return (tab)
    }
    typeSelector = (event) => {
        var tab = []
        for (var i = 0; i < this.state.type.length; i++) {

            tab.push(< option value={this.state.type[i].type} > {this.state.type[i].type}</option >)


        }
        return (tab)
    }

 
    render() {
        let x = this.colorsSelector()
        let y = this.typeSelector()
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePageFail">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('magazineManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true'){
            if (this.state.isLoading === true) {
                return (
                    <div className="Loading">
                        <ClipLoader loading={this.state.isLoading} size={150} />
                        <h1>Loading...</h1>
                    </div>
                )
            }
            else {
                return (
                    <div className="AddGlass">
                        <Sidebar />
                        <div className="title">
                            <h1 className="titletext">Add glass</h1>
                        </div>
                        <form>
                            <div className="AddGlass_c">

                                <div className="form-group">

                                    <label>Length (min. 200, max. 5000mm)</label>
                                    <input
                                        type="number"
                                        min="1"
                                        max="5000"
                                        name="Height"
                                        className="form-control"
                                        id="inputHeight"
                                        placeholder="Enter length in millimeters"
                                        ref="length"
                                    />
                                </div>
                                <div className="form-group">
                                    <label>Width (min. 200, max. 5000mm)</label>
                                    <input
                                        type="number"
                                        min="200"
                                        max="5000"
                                        className="form-control"
                                        id="inputWidth"
                                        placeholder="Enter width in millimeters"
                                        ref="width"
                                    />
                                </div>
                                <div className="form-group">
                                    <label>Thickness</label>
                                    <input
                                        type="number"
                                        min="1"
                                        className="form-control"
                                        id="inputLength"
                                        placeholder="Enter thickness in millimeters"
                                        ref="height"
                                    />
                                </div>
                                <div className="form-group">
                                    <label>Owner</label>
                                    <input
                                        type="text"
                                        className="form-control"
                                        id="inputOwner"
                                        placeholder="Enter owner name"
                                        ref="owner"
                                    />
                                </div>
                                <div className="form-group">
                                    <label>Color</label>
                                    <select ref="color" type="text" className="form-control">
                                        {x}
                                    </select>
                                </div>
                                <div className="form-group">
                                    <label>Type</label>
                                    <select ref="type" type="text" className="form-control">
                                        {y}
                                    </select>
                                </div>
                                <div className="form-group">
                                    <label>Shelf</label>
                                    <input
                                        type="text"
                                        className="form-control"
                                        id="inputDesk"
                                        placeholder="Enter shelf number"
                                        ref="desk"
                                    />
                                </div>
                                <div className="form-group">
                                    <label>Amount (min.1, max.100)</label>
                                    <input
                                        type="number"
                                        min="1"
                                        max="100"
                                        className="form-control"
                                        id="inputDesk"
                                        placeholder="Enter amount"
                                        ref="amount"
                                    />
                                </div>


                                <div className="form-group">

                                    <button type="button" className="success_glass_add" onClick={this.handleAddGlass}>Add glass</button>

                                    <button type="button" className="danger_glass_add" onClick={this.cancelAddGlass}>Cancel</button>


                                </div>


                            </div>
                        </form>
                    </div>


                );
            
            }
        }
        else {
            return (
                <div className="HomePageFail">
                    <h1>Check if you have perrmission to this panel</h1>
                    <button type="submit" className="success_login2" onClick={this.goback2} >Back to home page</button>
                </div>
            );

        }
    }
}