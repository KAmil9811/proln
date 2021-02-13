import React, { Component } from "react";
import './GlassEdit.css'
import Sidebar from '../Sidebar';

export class GlassEdit extends Component {
    displayName = GlassEdit.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            colors: [],
            type: [],
        }
    }
    
    componentDidMount() {
        var table2 = [];
        var table3 = [];
        fetch(`api/Magazine/Return_All_Colors`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
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
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
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

    handleGlassEdit = (event) => {
        event.preventDefault();
        const receiver = {
            glass: {
                type: this.refs.type.value,
                hight: this.refs.thickness.value,
                width: this.refs.width.value,
                length: this.refs.length.value,
                color: this.refs.color.value,
                owner: this.refs.owner.value,
                desk: sessionStorage.getItem('desk'),
                glass_Id: JSON.parse(sessionStorage.getItem('id')),
            },
            //glass_Id:{}
                       
            user: {
                login: sessionStorage.getItem('login')
            }
        }


        fetch(`api/Magazine/Edit_Glass`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
              
                return (json)
            })
        this.props.history.push('/glasswarehouse');

        
    }

    cancelGlassEdit = (event) => {
        sessionStorage.removeItem('length');
        sessionStorage.removeItem('width');
        sessionStorage.removeItem('thickness');
        sessionStorage.removeItem('color');
        sessionStorage.removeItem('type');
        sessionStorage.removeItem('amount');
        sessionStorage.removeItem('owner');
        this.props.history.push('/glasswarehouse');
    }


    colorsSelector = (event) => {
        var tab = []
        for (var i = 0; i < this.state.colors.length; i++) {

            tab.push(< option value={this.state.colors[i].color} > {this.state.colors[i].color}</option >)


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
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('magazineManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true'){
            return (
                <div className="GlassEdit">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Edit glass</h1>
                    </div>
                    <div className="GlassEdit_c">
                        <form>

                            <div className="glass_edit_conteiner">
                                <div className="form-group">

                                    <label>Length:</label>
                                    <input
                                        type="number"
                                        className="form-control"
                                        id="inputLength"
                                        placeholder={sessionStorage.getItem('length')}
                                        defaultValue={sessionStorage.getItem('length')}
                                        ref="length"
                                    />
                                </div>
                                <div className="form-group">
                                    <label>Width:</label>
                                    <input
                                        type="number"
                                        className="form-control"
                                        id="inputWidth"
                                        placeholder={sessionStorage.getItem('width')}
                                        defaultValue={sessionStorage.getItem('width')}
                                        ref="width"
                                    />
                                </div>
                                <div className="form-group">
                                    <label>Type:</label>
                                    <select
                                        type="text"
                                        className="form-control"
                                        placeholder={sessionStorage.getItem('type')}
                                        defaultValue={sessionStorage.getItem('type')}
                                        ref="type"
                                    >
                                        <option selected={sessionStorage.getItem('type')}> {sessionStorage.getItem('type')} </option>
                                        {y}
                                    </select>

                                </div>
                                <div className="form-group">
                                    <label>Thickness:</label>
                                    <input
                                        type="number"
                                        className="form-control"
                                        id="inputThickness"
                                        placeholder={sessionStorage.getItem('thickness')}
                                        defaultValue={sessionStorage.getItem('thickness')}
                                        ref="thickness"
                                    />
                                </div>
                                <div className="form-group">
                                    <label>Color:</label>
                                    <select
                                        type="text"
                                        className="form-control"
                                        id="inputColor"
                                        placeholder={sessionStorage.getItem('color')}
                                        defaultValue={sessionStorage.getItem('color')}
                                        ref="color"
                                    >
                                        <option selected={sessionStorage.getItem('color')}> {sessionStorage.getItem('color')} </option>
                                        {x}
                                    </select>
                                </div>
                                <div className="form-group">
                                    <label>Owner:</label>
                                    <input
                                        type="text"
                                        className="form-control"
                                        id="inputOwner"
                                        placeholder={sessionStorage.getItem('owner')}
                                        defaultValue={sessionStorage.getItem('owner')}
                                        ref="owner"
                                    />
                                </div>

                                <div >

                                    <button type="submit" className="success_glass_edit" onClick={this.handleGlassEdit}>Edit glass</button>

                                    <button type="submit" className="danger_glass_edit" onClick={this.cancelGlassEdit}>Cancel</button>


                                </div>
                            </div>

                        </form>
                    </div>
                </div>


                );
        }
        else {
            return (
                <div className="HomePage">
                    <h1>Check if you have perrmission to this panel</h1>
                    <button type="submit" className="success_login" onClick={this.goback2} >Back to home page</button>
                </div>
            );

        }
    }
}