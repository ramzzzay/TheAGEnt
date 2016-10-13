import DropzoneComponent from "react-dropzone-component";

module.exports = React.createClass({
    getInitialState: function() {
        return {
            componentConfig: {
                postUrl: this.props.url
            },
            djsConfig: {
                addRemoveLinks: true,
                headers: {
                    'Authorization': "bearer " + Cookie.load('tokenInfo')
                }
            },
            eventHandlers: {
                success: (file,response) => this.props.changeState(response.uploadedUrl)
            }
        };
    },
    render: function() {
        return (
            <div>
              <h3>Uploading {this.props.name || "image"}</h3>
                <DropzoneComponent config={this.state.componentConfig} eventHandlers={this.state.eventHandlers} djsConfig={this.state.djsConfig}/>
            </div>
        );
    }
  });
